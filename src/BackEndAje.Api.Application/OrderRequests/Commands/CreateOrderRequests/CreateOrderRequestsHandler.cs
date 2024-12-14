namespace BackEndAje.Api.Application.OrderRequests.Commands.CreateOrderRequests
{
    using AutoMapper;
    using BackEndAje.Api.Application.Dtos.OrderRequests;
    using BackEndAje.Api.Application.Hubs;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;
    using Microsoft.AspNetCore.SignalR;

    public class CreateOrderRequestsHandler : IRequestHandler<CreateOrderRequestsCommand, Unit>
    {
        private readonly IOrderRequestRepository _orderRequestRepository;
        private readonly IMastersRepository _mastersRepository;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IClientRepository _clientRepository;

        public CreateOrderRequestsHandler(IOrderRequestRepository orderRequestRepository, IMapper mapper, IMastersRepository mastersRepository, IHubContext<NotificationHub> hubContext, IUserRoleRepository userRoleRepository, INotificationRepository notificationRepository, IClientRepository clientRepository)
        {
            this._orderRequestRepository = orderRequestRepository;
            this._mapper = mapper;
            this._mastersRepository = mastersRepository;
            this._hubContext = hubContext;
            this._userRoleRepository = userRoleRepository;
            this._notificationRepository = notificationRepository;
            this._clientRepository = clientRepository;
        }

        public async Task<Unit> Handle(CreateOrderRequestsCommand request, CancellationToken cancellationToken)
        {
            if (request.ReasonRequestId == 5)
            {
                // Validar que DestinationClientId y AssetId tengan valores
                if (!request.DestinationClientId.HasValue || !request.AssetId.HasValue)
                {
                    throw new ArgumentException("DestinationClientId y AssetId son obligatorios para una reubicación.");
                }

                // Crear copia del request para Retiro (ReasonRequestId = 2)
                var withdrawalRequest = new CreateOrderRequestsCommand
                {
                    SupervisorId = request.SupervisorId,
                    CediId = request.CediId,
                    ReasonRequestId = 2,
                    NegotiatedDate = request.NegotiatedDate,
                    TimeWindowId = request.TimeWindowId,
                    WithDrawalReasonId = request.WithDrawalReasonId,
                    ClientId = request.ClientId,
                    ClientCode = request.ClientCode,
                    Observations = request.Observations,
                    Reference = request.Reference,
                    ProductSizeId = request.ProductSizeId,
                    AssetId = request.AssetId,
                    Documents = request.Documents,
                    CreatedBy = request.CreatedBy,
                    UpdatedBy = request.UpdatedBy,
                };

                var withdrawalOrderRequest = await this.CreateOrderRequestAsync(withdrawalRequest);
                await this.SaveOrderRequestStatusHistoryAsync(withdrawalOrderRequest.OrderRequestId, withdrawalOrderRequest.OrderStatusId, withdrawalRequest.CreatedBy);

                if (request.Documents != null && request.Documents.Any())
                {
                    await this.SaveOrderRequestDocumentsAsync(withdrawalOrderRequest.OrderRequestId, request.Documents, request.CreatedBy, request.UpdatedBy);
                }

                if (request.SupervisorId != null)
                {
                    var notificationMessage = this.GenerateNotificationMessage(withdrawalOrderRequest);
                    await this.NotifySupervisor(request.SupervisorId, notificationMessage.Result, cancellationToken);
                }

                await this.NotifyTrade(withdrawalOrderRequest, cancellationToken);

                var client = await this._clientRepository.GetClientById(request.DestinationClientId.Value);
                // Crear copia del request para Instalación (ReasonRequestId = 1)
                var installationRequest = new CreateOrderRequestsCommand
                {
                    SupervisorId = request.SupervisorId,
                    CediId = request.CediId,
                    ReasonRequestId = 1,
                    NegotiatedDate = request.NegotiatedDate,
                    TimeWindowId = request.TimeWindowId,
                    WithDrawalReasonId = request.WithDrawalReasonId,
                    ClientId = request.DestinationClientId!.Value, // El cliente de destino es el nuevo cliente
                    ClientCode = client!.ClientCode,
                    Observations = request.Observations,
                    Reference = request.Reference,
                    ProductSizeId = request.ProductSizeId,
                    AssetId = request.AssetId,
                    Documents = request.Documents,
                    CreatedBy = request.CreatedBy,
                    UpdatedBy = request.UpdatedBy,
                };

                var installationOrderRequest = await this.CreateOrderRequestAsync(installationRequest);
                await this.SaveOrderRequestStatusHistoryAsync(installationOrderRequest.OrderRequestId, installationOrderRequest.OrderStatusId, installationRequest.CreatedBy);

                if (request.Documents != null && request.Documents.Any())
                {
                    await this.SaveOrderRequestDocumentsAsync(installationOrderRequest.OrderRequestId, request.Documents, request.CreatedBy, request.UpdatedBy);
                }

                if (request.SupervisorId != null)
                {
                    var notificationMessage = this.GenerateNotificationMessage(installationOrderRequest);
                    await this.NotifySupervisor(request.SupervisorId, notificationMessage.Result, cancellationToken);
                }

                await this.NotifyTrade(installationOrderRequest, cancellationToken);

                // Crear Relocation y RelocationRequests
                var relocation = new Relocation
                {
                    OriginClientId = request.ClientId,
                    DestinationClientId = request.DestinationClientId.Value,
                    TransferredAssetId = request.AssetId.Value,
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = request.CreatedBy,
                    UpdatedBy = request.UpdatedBy,
                };
                await this._orderRequestRepository.AddRelocation(relocation);

                await this._orderRequestRepository.AddRelocationRequests(new RelocationRequest
                {
                    RelocationId = relocation.RelocationId,
                    ReasonRequestId = 2, // Retiro
                    OrderRequestId = withdrawalOrderRequest.OrderRequestId,
                    OrderStatusId = withdrawalOrderRequest.OrderStatusId,
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = request.CreatedBy,
                    UpdatedBy = request.UpdatedBy,
                });

                await this._orderRequestRepository.AddRelocationRequests(new RelocationRequest
                {
                    RelocationId = relocation.RelocationId,
                    ReasonRequestId = 1, // Instalación
                    OrderRequestId = installationOrderRequest.OrderRequestId,
                    OrderStatusId = installationOrderRequest.OrderStatusId,
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = request.CreatedBy,
                    UpdatedBy = request.UpdatedBy,
                });
            }
            else
            {
                // Crear orden normal si ReasonRequestId no es 5
                var orderRequest = await this.CreateOrderRequestAsync(request);
                await this.SaveOrderRequestStatusHistoryAsync(orderRequest.OrderRequestId, orderRequest.OrderStatusId, request.CreatedBy);

                if (request.Documents != null && request.Documents.Any())
                {
                    await this.SaveOrderRequestDocumentsAsync(orderRequest.OrderRequestId, request.Documents, request.CreatedBy, request.UpdatedBy);
                }

                if (request.SupervisorId != null)
                {
                    var notificationMessage = this.GenerateNotificationMessage(orderRequest);
                    await this.NotifySupervisor(request.SupervisorId, notificationMessage.Result, cancellationToken);
                }

                await this.NotifyTrade(orderRequest, cancellationToken);
            }

            return Unit.Value;
        }

        #region Private Methods

        private async Task<OrderRequest> CreateOrderRequestAsync(CreateOrderRequestsCommand request)
        {
            var orderRequest = this._mapper.Map<OrderRequest>(request);
            var status = await this._mastersRepository.GetAllOrderStatus();
            var statusInitial = status.FirstOrDefault(x => x.StatusName == "GENERADO")!.OrderStatusId;

            orderRequest.OrderStatusId = statusInitial;
            orderRequest.IsActive = true;

            await this._orderRequestRepository.AddOrderRequestAsync(orderRequest);

            return orderRequest;
        }

        private async Task SaveOrderRequestStatusHistoryAsync(int orderRequestId, int orderStatusId, int createdBy)
        {
            var orderRequestStatusHistory = new OrderRequestStatusHistory
            {
                OrderRequestId = orderRequestId,
                OrderStatusId = orderStatusId,
                ChangeReason = null,
                CreatedAt = DateTime.Now,
                CreatedBy = createdBy,
            };

            await this._orderRequestRepository.AddOrderRequestStatusHistoryAsync(orderRequestStatusHistory);
        }

        private async Task SaveOrderRequestDocumentsAsync(int orderRequestId, List<CreateOrderRequestDocumentDto> documents, int createdBy, int updatedBy)
        {
            var orderRequestDocuments = this._mapper.Map<List<OrderRequestDocument>>(documents);

            foreach (var document in orderRequestDocuments)
            {
                document.OrderRequestId = orderRequestId;
                document.IsActive = true;
                document.CreatedBy = createdBy;
                document.UpdatedBy = updatedBy;

                await this._orderRequestRepository.AddOrderRequestDocumentAsync(document);
            }
        }

        private async Task<string> GenerateNotificationMessage(OrderRequest orderRequest)
        {
            var reasonRequest = await this._mastersRepository.GetAllReasonRequest();
            var reasonDescription = reasonRequest.FirstOrDefault(r => r.ReasonRequestId == orderRequest.ReasonRequestId)!.ReasonDescription;
            return $"Se ha generado una solicitud de {reasonDescription} para el cliente con código: {orderRequest.ClientCode}.";
        }

        private async Task NotifySupervisor(int supervisorUserId, string message, CancellationToken cancellationToken)
        {
            var notification = new Notification
            {
                UserId = supervisorUserId,
                Message = message,
                IsRead = false,
                CreatedAt = DateTime.Now,
            };

            await this._notificationRepository.AddNotificationAsync(notification);
            var notificationId = notification.Id;
            await this._hubContext.Clients.User(supervisorUserId.ToString())
                .SendAsync("ReceiveMessage", notificationId, message, cancellationToken: cancellationToken);
        }

        private async Task NotifyTrade(OrderRequest orderRequest, CancellationToken cancellationToken)
        {
            var userRoles = await this._userRoleRepository.GetUserRolesByTradeAsync();

            foreach (var user in userRoles)
            {
                var notificationMessage = this.GenerateNotificationMessage(orderRequest);

                // Notificar a través de SignalR
                await this._hubContext.Clients.User(user.UserId.ToString())
                    .SendAsync("ReceiveMessage", "Sistema", notificationMessage, cancellationToken);

                var notification = new Notification
                {
                    UserId = user.UserId,
                    Message = notificationMessage.Result,
                    IsRead = false,
                    CreatedAt = DateTime.Now,
                };

                await this._notificationRepository.AddNotificationAsync(notification);
            }
        }

        #endregion
    }
}
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

        public CreateOrderRequestsHandler(IOrderRequestRepository orderRequestRepository, IMapper mapper, IMastersRepository mastersRepository, IHubContext<NotificationHub> hubContext, IUserRoleRepository userRoleRepository, INotificationRepository notificationRepository)
        {
            this._orderRequestRepository = orderRequestRepository;
            this._mapper = mapper;
            this._mastersRepository = mastersRepository;
            this._hubContext = hubContext;
            this._userRoleRepository = userRoleRepository;
            this._notificationRepository = notificationRepository;
        }

        public async Task<Unit> Handle(CreateOrderRequestsCommand request, CancellationToken cancellationToken)
        {
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
            await this._hubContext.Clients.User(supervisorUserId.ToString())
                .SendAsync("ReceiveMessage", "Sistema", message, cancellationToken: cancellationToken);

            var notification = new Notification
            {
                UserId = supervisorUserId,
                Message = message,
                IsRead = false,
                CreatedAt = DateTime.Now,
            };

            await this._notificationRepository.AddNotificationAsync(notification);
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
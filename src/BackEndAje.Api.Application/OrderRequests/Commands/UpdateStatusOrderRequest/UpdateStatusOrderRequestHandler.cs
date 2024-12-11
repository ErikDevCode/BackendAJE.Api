namespace BackEndAje.Api.Application.OrderRequests.Commands.UpdateStatusOrderRequest
{
    using BackEndAje.Api.Application.Dtos.Const;
    using BackEndAje.Api.Application.Hubs;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;
    using Microsoft.AspNetCore.SignalR;

    public class UpdateStatusOrderRequestHandler : IRequestHandler<UpdateStatusOrderRequestCommand, Unit>
    {
        private readonly IOrderRequestRepository _orderRequestRepository;
        private readonly IClientAssetRepository _clientAssetRepository;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IMastersRepository _mastersRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRoleRepository _userRoleRepository;

        public UpdateStatusOrderRequestHandler(IOrderRequestRepository orderRequestRepository, IClientAssetRepository clientAssetRepository, IHubContext<NotificationHub> hubContext, IMastersRepository mastersRepository, INotificationRepository notificationRepository, IUserRoleRepository userRoleRepository)
        {
            this._orderRequestRepository = orderRequestRepository;
            this._clientAssetRepository = clientAssetRepository;
            this._hubContext = hubContext;
            this._mastersRepository = mastersRepository;
            this._notificationRepository = notificationRepository;
            this._userRoleRepository = userRoleRepository;
        }

        public async Task<Unit> Handle(UpdateStatusOrderRequestCommand request, CancellationToken cancellationToken)
        {
            var orderRequest = await this._orderRequestRepository.GetOrderRequestById(request.OrderRequestId);
            if (orderRequest!.OrderStatusId == (int)OrderStatusConst.Generado && request.OrderStatusId == (int)OrderStatusConst.Atendido)
            {
                throw new InvalidOperationException("La solicitud no se puede atender porque no ha sido aprobada");
            }

            this.ValidateApprovalAsset(orderRequest);
            this.ValidateApproval(orderRequest, request.OrderStatusId);

            await this.UpdateClientAssets(request, orderRequest);

            await this.UpdateOrderRequestStatus(request);

            if (request.OrderStatusId != (int)OrderStatusConst.Aprobado)
            {
                await this.NotifySupervisor(orderRequest, request.OrderStatusId, cancellationToken);
            }

            if (request.OrderStatusId == (int)OrderStatusConst.Aprobado)
            {
                await this.NotifyLogisticsProviders(orderRequest, request.OrderStatusId, cancellationToken);
            }

            return Unit.Value;
        }

        #region Private Methods

        private void ValidateApprovalAsset(OrderRequest orderRequest)
        {
            if (orderRequest.OrderRequestAssets == null || orderRequest.OrderRequestAssets.Count == 0)
            {
                throw new InvalidOperationException("No se puede aprobar la solicitud sin Activos");
            }
        }

        private void ValidateApproval(OrderRequest orderRequest, int orderStatusId)
        {
            if (orderStatusId == (int)OrderStatusConst.Aprobado && !orderRequest.OrderRequestDocuments.Any())
            {
                throw new InvalidOperationException("No se puede aprobar la solicitud sin documentos adjuntos.");
            }
        }

        private async Task UpdateClientAssets(UpdateStatusOrderRequestCommand request, OrderRequest orderRequest)
        {
            foreach (var orderRequestAsset in orderRequest.OrderRequestAssets)
            {
                var clientAssetDto = await this._clientAssetRepository
                    .GetClientAssetPendingApprovalByClientIdAndAssetIdAsync(orderRequest.ClientId, orderRequestAsset.AssetId);

                if (request.OrderStatusId == (int)OrderStatusConst.Rechazado && clientAssetDto == null)
                {
                    continue;
                }

                if (!IsValidOrderStatus(request.OrderStatusId))
                {
                    continue;
                }

                var clientAsset = this.CreateClientAssetFromDto(clientAssetDto, request);
                clientAsset.IsActive = request.OrderStatusId == (int)OrderStatusConst.Atendido;
                clientAsset.Notes = this.GetStatusNotes(request.OrderStatusId);

                await this._clientAssetRepository.UpdateClientAssetsAsync(clientAsset);
            }
        }

        private async Task NotifySupervisor(OrderRequest orderRequest, int orderStatusId, CancellationToken cancellationToken)
        {
            if (orderRequest.Supervisor == null) return;

            var orderStatus = await this._mastersRepository.GetAllOrderStatus();
            var orderStatusName = orderStatus.FirstOrDefault(s => s.OrderStatusId == orderStatusId)?.StatusName;
            var notificationMessage = $"Te informamos que la solicitud Nro. {orderRequest.OrderRequestId}, asociada al cliente con el código {orderRequest.ClientCode}, ha sido actualizada al estado: {orderStatusName}.";

            var notification = new Notification
            {
                UserId = orderRequest.Supervisor.UserId,
                Message = notificationMessage,
                IsRead = false,
                CreatedAt = DateTime.Now,
            };
            await this._notificationRepository.AddNotificationAsync(notification);
            var notificationId = notification.Id;
            await this._hubContext.Clients.User(orderRequest.Supervisor.UserId.ToString())
                .SendAsync("ReceiveMessage", notificationId, notificationMessage, cancellationToken);
        }

        private async Task NotifyLogisticsProviders(OrderRequest orderRequest, int orderStatusId, CancellationToken cancellationToken)
        {
            var userRoles = await this._userRoleRepository.GetUserRolesByLogisticsProviderAsync();
            var orderStatus = await this._mastersRepository.GetAllOrderStatus();
            var orderStatusName = orderStatus.FirstOrDefault(s => s.OrderStatusId == orderStatusId)?.StatusName;
            foreach (var user in userRoles)
            {
                var notificationMessage = $"Te informamos que la solicitud Nro. {orderRequest.OrderRequestId}, asociada al cliente con el código {orderRequest.ClientCode}, ha sido actualizada al estado: {orderStatusName}.";
                var notification = new Notification
                {
                    UserId = user.UserId,
                    Message = notificationMessage,
                    IsRead = false,
                    CreatedAt = DateTime.Now,
                };
                await this._notificationRepository.AddNotificationAsync(notification);
                var notificationId = notification.Id;

                // Notificar a través de SignalR
                await this._hubContext.Clients.User(user.UserId.ToString())
                    .SendAsync("ReceiveMessage", notificationId, notificationMessage, cancellationToken);
            }
        }

        private ClientAssets CreateClientAssetFromDto(ClientAssets clientAssetDto, UpdateStatusOrderRequestCommand request)
        {
            return new ClientAssets
            {
                ClientAssetId = clientAssetDto.ClientAssetId,
                CediId = clientAssetDto.CediId,
                InstallationDate = request.StatusDate,
                ClientId = clientAssetDto.ClientId,
                AssetId = clientAssetDto.AssetId,
                CodeAje = clientAssetDto.CodeAje,
                CreatedAt = clientAssetDto.CreatedAt,
                CreatedBy = clientAssetDto.CreatedBy,
                UpdatedAt = DateTime.Now,
                UpdatedBy = request.CreatedBy,
            };
        }

        private bool IsValidOrderStatus(int orderStatusId)
        {
            return orderStatusId == (int)OrderStatusConst.Atendido ||
                   orderStatusId == (int)OrderStatusConst.Rechazado ||
                   orderStatusId == (int)OrderStatusConst.FalsoFlete ||
                   orderStatusId == (int)OrderStatusConst.Anulado;
        }

        private string GetStatusNotes(int orderStatusId)
        {
            return orderStatusId switch
            {
                (int)OrderStatusConst.Atendido => "Activo Atendido",
                (int)OrderStatusConst.Rechazado => "Activo Rechazado",
                (int)OrderStatusConst.FalsoFlete => "Activo tiene Falso Flete",
                (int)OrderStatusConst.Anulado => "Activo está Anulado",
                _ => throw new ArgumentOutOfRangeException(nameof(orderStatusId), orderStatusId, null),
            };
        }

        private async Task UpdateOrderRequestStatus(UpdateStatusOrderRequestCommand request)
        {
            await this._orderRequestRepository.UpdateStatusOrderRequestAsync(request.OrderRequestId, request.OrderStatusId, request.CreatedBy);

            var orderRequestStatusHistory = new OrderRequestStatusHistory
            {
                OrderRequestId = request.OrderRequestId,
                OrderStatusId = request.OrderStatusId,
                ChangeReason = request.ChangeReason,
                CreatedAt = DateTime.Now,
                CreatedBy = request.CreatedBy,
            };

            await this._orderRequestRepository.AddOrderRequestStatusHistoryAsync(orderRequestStatusHistory);
        }

        #endregion
    }
}

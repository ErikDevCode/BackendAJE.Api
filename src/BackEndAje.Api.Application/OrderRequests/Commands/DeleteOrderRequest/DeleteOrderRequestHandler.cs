namespace BackEndAje.Api.Application.OrderRequests.Commands.DeleteOrderRequest
{
    using BackEndAje.Api.Application.Hubs;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;
    using Microsoft.AspNetCore.SignalR;

    public class DeleteOrderRequestHandler : IRequestHandler<DeleteOrderRequestCommand, Unit>
    {
        private readonly IOrderRequestRepository _orderRequestRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IClientAssetRepository _clientAssetRepository;

        public DeleteOrderRequestHandler(IOrderRequestRepository orderRequestRepository, INotificationRepository notificationRepository, IHubContext<NotificationHub> hubContext, IClientAssetRepository clientAssetRepository)
        {
            this._orderRequestRepository = orderRequestRepository;
            this._notificationRepository = notificationRepository;
            this._hubContext = hubContext;
            this._clientAssetRepository = clientAssetRepository;
        }

        public async Task<Unit> Handle(DeleteOrderRequestCommand request, CancellationToken cancellationToken)
        {
            var orderRequest = await this._orderRequestRepository.GetOrderRequestById(request.OrderRequestId);

            if (orderRequest == null)
            {
                throw new KeyNotFoundException($"Solicitud con ID {request.OrderRequestId} no encontrada.");
            }

            var relocationRequest = await this._orderRequestRepository.GetListRelocationRequestByOrderRequestId(orderRequest.OrderRequestId);
            if (relocationRequest?.Any() == true)
            {
                foreach (var relocationReq in relocationRequest)
                {
                    var relocation = await this._orderRequestRepository.GetRelocationById(relocationReq.RelocationId);
                    relocation.IsActive = false;
                    relocation.UpdatedAt = DateTime.Now;
                    relocation.UpdatedBy = request.UpdatedBy;
                    await this._orderRequestRepository.DeleteRelocationAsync(relocation);

                    relocationReq.IsActive = false;
                    relocationReq.UpdatedAt = DateTime.Now;
                    relocationReq.UpdatedBy = request.UpdatedBy;
                    await this._orderRequestRepository.DeleteRelocationRequestAsync(relocationReq);
                }
            }

            // Marcar la solicitud como inactiva
            this.MarkOrderRequestAsInactive(orderRequest, request.UpdatedBy);
            await this._orderRequestRepository.DeleteOrderRequestAsync(orderRequest);


            // Procesar los activos relacionados
            if (orderRequest.OrderRequestAssets != null && orderRequest.OrderRequestAssets.Any())
            {
                await this.ProcessOrderRequestAssets(orderRequest, request.UpdatedBy);
            }

            // Enviar notificación al supervisor
            var notificationMessage = this.GenerateNotificationMessage(orderRequest);
            await this.NotifySupervisor(orderRequest.SupervisorId, notificationMessage.Result, cancellationToken);
            return Unit.Value;
        }

        private async Task ProcessOrderRequestAssets(OrderRequest orderRequest, int updatedBy)
        {
            foreach (var requestAsset in orderRequest.OrderRequestAssets)
            {
                // Marcar el activo de la solicitud como inactivo
                var orderRequestAsset = new OrderRequestAssets
                {
                    OrderRequestAssetId = requestAsset.OrderRequestAssetId,
                    OrderRequestId = requestAsset.OrderRequestId,
                    AssetId = requestAsset.AssetId,
                    IsActive = false,
                };

                await this._orderRequestRepository.UpdateAssetToOrderRequest(orderRequestAsset);

                // Verificar si el activo está relacionado con un cliente
                var clientAsset = await this._clientAssetRepository.GetClientAssetByClientIdAndAssetId(orderRequest.ClientId, requestAsset.AssetId);
                if (clientAsset != null)
                {
                    this.UpdateClientAssetProperties(clientAsset, "Solicitud eliminada", updatedBy);
                    await this._clientAssetRepository.UpdateClientAssetsAsync(clientAsset);
                }
            }
        }

        private void MarkOrderRequestAsInactive(OrderRequest orderRequest, int updatedBy)
        {
            orderRequest.IsActive = false;
            orderRequest.UpdatedAt = DateTime.Now;
            orderRequest.UpdatedBy = updatedBy;
        }

        private void UpdateClientAssetProperties(ClientAssets clientAsset, string notes, int updatedBy)
        {
            clientAsset.IsActive = false;
            clientAsset.Notes = notes;
            clientAsset.UpdatedAt = DateTime.Now;
            clientAsset.UpdatedBy = updatedBy;
        }

        private async Task NotifySupervisor(int supervisorUserId, string message, CancellationToken cancellationToken)
        {
            var notification = new Domain.Entities.Notification
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

        private Task<string> GenerateNotificationMessage(OrderRequest orderRequest)
        {
            return Task.FromResult($"Se ha eliminado una solicitud para el cliente con código: {orderRequest.ClientCode}.");
        }
    }
}


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

        public DeleteOrderRequestHandler(IOrderRequestRepository orderRequestRepository, INotificationRepository notificationRepository, IHubContext<NotificationHub> hubContext)
        {
            this._orderRequestRepository = orderRequestRepository;
            this._notificationRepository = notificationRepository;
            this._hubContext = hubContext;
        }

        public async Task<Unit> Handle(DeleteOrderRequestCommand request, CancellationToken cancellationToken)
        {
            var orderRequest = await this._orderRequestRepository.GetOrderRequestById(request.OrderRequestId);

            if (orderRequest == null)
            {
                throw new KeyNotFoundException($"Solicitud con ID {request.OrderRequestId} no encontrada.");
            }

            orderRequest.IsActive = false;
            orderRequest.UpdatedAt = DateTime.Now;
            orderRequest.UpdatedBy = request.UpdatedBy;
            await this._orderRequestRepository.DeleteOrderRequestAsync(orderRequest);
            var notificationMessage = this.GenerateNotificationMessage(orderRequest);
            await this.NotifySupervisor(orderRequest.SupervisorId, notificationMessage.Result, cancellationToken);
            return Unit.Value;
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


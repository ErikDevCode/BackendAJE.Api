namespace BackEndAje.Api.Application.Services
{
    using BackEndAje.Api.Application.Hubs;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using Microsoft.AspNetCore.SignalR;

    public class NotificationNewService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IMastersRepository _mastersRepository;

        public NotificationNewService(
            IHubContext<NotificationHub> hubContext,
            INotificationRepository notificationRepository,
            IUserRoleRepository userRoleRepository,
            IMastersRepository mastersRepository)
        {
            this._hubContext = hubContext;
            this._notificationRepository = notificationRepository;
            this._userRoleRepository = userRoleRepository;
            this._mastersRepository = mastersRepository;
        }

        public async Task NotifySupervisorAsync(int supervisorUserId, string message, CancellationToken cancellationToken)
        {
            var notification = new Domain.Entities.Notification
            {
                UserId = supervisorUserId,
                Message = message,
                IsRead = false,
                CreatedAt = DateTime.Now,
            };

            await this._notificationRepository.AddNotificationAsync(notification);
            await this._hubContext.Clients.User(supervisorUserId.ToString())
                .SendAsync("ReceiveMessage", notification.Id, message, cancellationToken: cancellationToken);
        }

        public async Task NotifyTradeAsync(OrderRequest orderRequest, string message, CancellationToken cancellationToken)
        {
            var userRoles = await this._userRoleRepository.GetUserRolesByTradeAsync();

            foreach (var user in userRoles)
            {
                var notification = new Domain.Entities.Notification
                {
                    UserId = user.UserId,
                    Message = message,
                    IsRead = false,
                    CreatedAt = DateTime.Now,
                };

                await this._notificationRepository.AddNotificationAsync(notification);
                var notificationId = notification.Id;
                await this._hubContext.Clients.User(user.UserId.ToString())
                    .SendAsync("ReceiveMessage", notificationId, message, cancellationToken);
            }
        }

        public async Task<string> GenerateNotificationMessage(OrderRequest orderRequest)
        {
            var reasonRequest = await this._mastersRepository.GetAllReasonRequest();
            var reasonDescription = reasonRequest
                                        .FirstOrDefault(r => r.ReasonRequestId == orderRequest.ReasonRequestId)?.ReasonDescription
                                    ?? "una razón desconocida";

            return $"Se ha generado una solicitud de {reasonDescription} para el cliente con código: {orderRequest.Client.ClientCode}.";
        }
    }
}


namespace BackEndAje.Api.Application.Services
{
    using BackEndAje.Api.Application.Hubs;
    using Microsoft.AspNetCore.SignalR;

    public class NotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            this._hubContext = hubContext;
        }

        public async Task NotifyClients(string user, string message)
        {
            await this._hubContext.Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}

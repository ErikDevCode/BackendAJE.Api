namespace BackEndAje.Api.Application.Hubs
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;

    [Authorize]
    public class NotificationHub : Hub
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst("sub")?.Value; // Obtén el ID del usuario desde el JWT
            if (userId != null)
            {
                await this.Groups.AddToGroupAsync(this.Context.ConnectionId, userId); // Asocia la conexión al grupo del usuario
            }

            await base.OnConnectedAsync();
        }

        // Método para limpiar grupos al desconectarse
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = this.Context.User?.FindFirst("sub")?.Value;
            if (userId != null)
            {
                await this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, userId);
            }

            await base.OnDisconnectedAsync(exception);
        }

        public NotificationHub(IHubContext<NotificationHub> hubContext)
        {
            this._hubContext = hubContext;
        }

        public async Task ForwardMessage(string userId, string message, string notificationId)
        {
            await SendMessage(this._hubContext, userId, message, notificationId);
        }

        private static async Task SendMessage(IHubContext<NotificationHub> hubContext, string userId, string message, string notificationId)
        {
            await hubContext.Clients.User(userId).SendAsync("ReceiveMessage", message, notificationId);
        }
    }
}


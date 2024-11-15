namespace BackEndAje.Api.Application.Notification.Commands.SendNotification
{
    using BackEndAje.Api.Application.Hubs;
    using MediatR;
    using Microsoft.AspNetCore.SignalR;

    public class SendNotificationHandler : IRequestHandler<SendNotificationCommand>
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public SendNotificationHandler(IHubContext<NotificationHub> hubContext)
        {
            this._hubContext = hubContext;
        }

        public async Task<Unit> Handle(SendNotificationCommand request, CancellationToken cancellationToken)
        {
            await this._hubContext.Clients.All.SendAsync("ReceiveMessage", request.User, request.Message, cancellationToken: cancellationToken);
            return Unit.Value;
        }
    }
}


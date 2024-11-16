namespace BackEndAje.Api.Application.Notification.Commands.NotificationMarkAsRead
{
    using MediatR;

    public class NotificationMarkAsReadCommand : IRequest<Unit>
    {
        public int NotificationId { get; set; }
    }
}


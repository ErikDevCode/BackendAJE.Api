namespace BackEndAje.Api.Application.Notification.Commands.CreateNotification
{
    using MediatR;

    public class CreateNotificationCommand : IRequest<Unit>
    {
        public int UserId { get; set; }

        public string Message { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}

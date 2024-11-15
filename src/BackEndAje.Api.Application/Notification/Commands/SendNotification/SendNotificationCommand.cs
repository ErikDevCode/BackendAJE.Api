namespace BackEndAje.Api.Application.Notification.Commands.SendNotification
{
    using MediatR;

    public class SendNotificationCommand : IRequest
    {
        public string User { get; set; }

        public string Message { get; set; }
    }
}


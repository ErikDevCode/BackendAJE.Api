namespace BackEndAje.Api.Application.Notification.Commands.NotificationMarkAsRead
{
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class NotificationMarkAsReadHandler : IRequestHandler<NotificationMarkAsReadCommand, Unit>
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationMarkAsReadHandler(INotificationRepository notificationRepository)
        {
            this._notificationRepository = notificationRepository;
        }

        public async Task<Unit> Handle(NotificationMarkAsReadCommand request, CancellationToken cancellationToken)
        {
            await this._notificationRepository.MarkAsReadAsync(request.NotificationId);
            return Unit.Value;
        }
    }
}
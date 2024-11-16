namespace BackEndAje.Api.Application.Notification.Commands.CreateNotification
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class CreateNotificationHandler : IRequestHandler<CreateNotificationCommand, Unit>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;

        public CreateNotificationHandler(INotificationRepository notificationRepository, IMapper mapper)
        {
            this._notificationRepository = notificationRepository;
            this._mapper = mapper;
        }

        public async Task<Unit> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            var newNotification = this._mapper.Map<Domain.Entities.Notification>(request);
            await this._notificationRepository.AddNotificationAsync(newNotification);
            return Unit.Value;
        }
    }
}
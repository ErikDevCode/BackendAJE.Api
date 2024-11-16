namespace BackEndAje.Api.Application.Notification.Queries.GetNotificationByUserId
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetNotificationByUserIdHandler : IRequestHandler<GetNotificationByUserIdQuery, List<GetNotificationByUserIdResult>>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;

        public GetNotificationByUserIdHandler(INotificationRepository notificationRepository, IMapper mapper)
        {
            this._notificationRepository = notificationRepository;
            this._mapper = mapper;
        }

        public async Task<List<GetNotificationByUserIdResult>> Handle(GetNotificationByUserIdQuery request, CancellationToken cancellationToken)
        {
            var permissionsWithActions = await this._notificationRepository.GetAllNotificationAsync(request.userId);
            return this._mapper.Map<List<GetNotificationByUserIdResult>>(permissionsWithActions);
        }
    }
}
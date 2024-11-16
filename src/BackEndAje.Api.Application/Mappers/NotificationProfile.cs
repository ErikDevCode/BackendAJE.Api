namespace BackEndAje.Api.Application.Mappers
{
    using AutoMapper;
    using BackEndAje.Api.Application.Notification.Commands.CreateNotification;
    using BackEndAje.Api.Application.Notification.Queries.GetNotificationByUserId;

    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            this.CreateMap<CreateNotificationCommand, Domain.Entities.Notification>();

            this.CreateMap<Domain.Entities.Notification, GetNotificationByUserIdResult>();
        }
    }
}


namespace BackEndAje.Api.Application.Notification.Queries.GetNotificationByUserId
{
    using MediatR;

    public record GetNotificationByUserIdQuery(int userId) : IRequest<List<GetNotificationByUserIdResult>>;
}

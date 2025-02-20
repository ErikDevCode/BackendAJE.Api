namespace BackEndAje.Api.Application.Dashboard.Queries.GetAssetsFromOrderRequestStatusAttendedByUserId
{
    using BackEndAje.Api.Application.Behaviors;
    using MediatR;

    public record GetAssetsFromOrderRequestStatusAttendedByUserIdQuery(int? regionId, int? cediId, int? zoneId, int? route, int? month, int? year) : IRequest<GetAssetsFromOrderRequestStatusAttendedByUserIdResult>, IHasUserId
    {
        public int UserId { get; set; }
    }
}
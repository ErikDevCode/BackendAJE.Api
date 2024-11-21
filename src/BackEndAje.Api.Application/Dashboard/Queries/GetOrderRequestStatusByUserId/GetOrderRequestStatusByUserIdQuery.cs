namespace BackEndAje.Api.Application.Dashboard.Queries.GetOrderRequestStatusByUserId
{
    using BackEndAje.Api.Application.Behaviors;
    using MediatR;

    public record GetOrderRequestStatusByUserIdQuery(int? regionId, int? zoneId, int? route, int? month, int? year)
        : IRequest<List<GetOrderRequestStatusByUserIdResult>>, IHasUserId
    {
        public int UserId { get; set; }
    }
}


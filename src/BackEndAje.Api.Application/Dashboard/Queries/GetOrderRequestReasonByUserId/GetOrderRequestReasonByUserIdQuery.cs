namespace BackEndAje.Api.Application.Dashboard.Queries.GetOrderRequestReasonByUserId
{
    using BackEndAje.Api.Application.Behaviors;
    using MediatR;

    public record GetOrderRequestReasonByUserIdQuery(int? regionId, int? zoneId, int? route, int? month, int? year) : IRequest<List<GetOrderRequestReasonByUserIdResult>>, IHasUserId
    {
        public int UserId { get; set; }
    }
}

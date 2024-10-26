namespace BackEndAje.Api.Application.Dashboard.Queries.GetOrderRequestReasonByUserId
{
    using MediatR;

    public record GetOrderRequestReasonByUserIdQuery(int userId, int? regionId, int? zoneId, int? route, int? month, int? year) : IRequest<List<GetOrderRequestReasonByUserIdResult>>;
}

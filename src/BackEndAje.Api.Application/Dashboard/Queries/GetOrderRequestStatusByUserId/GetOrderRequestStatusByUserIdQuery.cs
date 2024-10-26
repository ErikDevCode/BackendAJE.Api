namespace BackEndAje.Api.Application.Dashboard.Queries.GetOrderRequestStatusByUserId
{
    using MediatR;

    public record GetOrderRequestStatusByUserIdQuery(int userId, int? regionId, int? zoneId, int? route, int? month, int? year) : IRequest<List<GetOrderRequestStatusByUserIdResult>>;
}


namespace BackEndAje.Api.Application.Users.Queries.GetUserByRouteOrEmail
{
    using MediatR;

    public record GetUserByRouteIdOrEmailQuery(string RouteIdOrEmail) : IRequest<GetUserByRouteIdOrEmailResult>;
}

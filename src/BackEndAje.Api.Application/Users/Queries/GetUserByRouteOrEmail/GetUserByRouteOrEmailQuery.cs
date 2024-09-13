namespace BackEndAje.Api.Application.Users.Queries.GetUserByRouteOrEmail
{
    using MediatR;

    public record GetUserByRouteOrEmailQuery(string RouteOrEmail) : IRequest<GetUserByRouteOrEmailResult>;
}

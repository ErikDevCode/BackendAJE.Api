namespace BackEndAje.Api.Application.Locations.Queries.GetCedisByUserId
{
    using MediatR;

    public record GetCedisByUserIdQuery(int userId) : IRequest<List<GetCedisByUserIdResult>>;
}

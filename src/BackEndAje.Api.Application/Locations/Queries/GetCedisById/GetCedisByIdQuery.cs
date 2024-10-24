namespace BackEndAje.Api.Application.Locations.Queries.GetCedisById
{
    using MediatR;

    public record GetCedisByIdQuery(int cediId) : IRequest<GetCedisByIdResult>;
}
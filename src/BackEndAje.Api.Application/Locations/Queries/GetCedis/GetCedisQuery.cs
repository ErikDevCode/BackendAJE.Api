namespace BackEndAje.Api.Application.Locations.Queries.GetCedis
{
    using MediatR;

    public record GetCedisQuery() : IRequest<List<GetCedisResult>>;
}


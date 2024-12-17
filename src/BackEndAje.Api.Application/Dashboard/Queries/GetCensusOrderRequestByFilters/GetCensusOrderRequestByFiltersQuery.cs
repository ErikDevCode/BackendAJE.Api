namespace BackEndAje.Api.Application.Dashboard.Queries.GetCensusOrderRequestByFilters
{
    using BackEndAje.Api.Application.Behaviors;
    using MediatR;

    public record GetCensusOrderRequestByFiltersQuery(int? regionId, int? zoneId, int? route, int? month, int? year) : IRequest<GetCensusOrderRequestByFiltersResult>, IHasUserId
    {
        public int UserId { get; set; }
    }
}


namespace BackEndAje.Api.Application.Locations.Queries.GetCedisByRegionId
{
    using MediatR;

    public class GetCedisByRegionIdQuery : IRequest<List<GetCedisByRegionIdResult>>
    {
        public int RegionId { get; set; }
    }
}


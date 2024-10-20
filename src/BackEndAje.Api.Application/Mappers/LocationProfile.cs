namespace BackEndAje.Api.Application.Mappers
{
    using AutoMapper;
    using BackEndAje.Api.Application.Locations.Queries.GetCedisByRegionId;
    using BackEndAje.Api.Application.Locations.Queries.GetRegions;
    using BackEndAje.Api.Application.Locations.Queries.GetZoneByCediId;
    using BackEndAje.Api.Domain.Entities;

    public class LocationProfile : Profile
    {
        public LocationProfile()
        {
            this.CreateMap<Region, GetRegionsResult>();

            this.CreateMap<Cedi, GetCedisByRegionIdResult>();

            this.CreateMap<Zone, GetZoneByCediIdResult>();
        }
    }
}

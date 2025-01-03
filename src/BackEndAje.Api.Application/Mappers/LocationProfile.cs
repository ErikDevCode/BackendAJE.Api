namespace BackEndAje.Api.Application.Mappers
{
    using AutoMapper;
    using BackEndAje.Api.Application.Locations.Queries.GetCedis;
    using BackEndAje.Api.Application.Locations.Queries.GetCedisById;
    using BackEndAje.Api.Application.Locations.Queries.GetCedisByRegionId;
    using BackEndAje.Api.Application.Locations.Queries.GetCedisByUserId;
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

            this.CreateMap<Cedi, GetCedisByIdResult>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CediId))
                .ForMember(dest => dest.CediName, opt => opt.MapFrom(src => src.CediName));

            this.CreateMap<Cedi, GetCedisByUserIdResult>();

            this.CreateMap<Cedi, GetCedisResult>()
                .ForMember(dest => dest.CediId, opt => opt.MapFrom(src => src.CediId))
                .ForMember(dest => dest.RegionId, opt => opt.MapFrom(src => src.RegionId))
                .ForMember(dest => dest.CediName, opt => opt.MapFrom(src => src.CediName));
        }
    }
}

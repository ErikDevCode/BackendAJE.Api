namespace BackEndAje.Api.Application.Mappers
{
    using AutoMapper;
    using BackEndAje.Api.Application.Dtos.Users;
    using BackEndAje.Api.Application.Users.Queries.GetUserByRouteOrEmail;
    using BackEndAje.Api.Domain.Entities;

    public class UserProfile : Profile
    {
        public UserProfile()
        {
            this.CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.CediId, opt => opt.MapFrom(src => src.CediId > 0 ? src.CediId : null))
                .ForMember(dest => dest.ZoneId, opt => opt.MapFrom(src => src.ZoneId > 0 ? src.ZoneId : null))
                .ForMember(dest => dest.Route, opt => opt.MapFrom(src => src.Route > 0 ? src.Route : null))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code > 0 ? src.Code : null));

            this.CreateMap<UpdateUserDto, User>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            this.CreateMap<User, GetUserByRouteOrEmailResult>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.CediId, opt => opt.MapFrom(src => src.CediId))
                .ForMember(dest => dest.ZoneId, opt => opt.MapFrom(src => src.ZoneId))
                .ForMember(dest => dest.Route, opt => opt.MapFrom(src => src.Route))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.PaternalSurName, opt => opt.MapFrom(src => src.PaternalSurName))
                .ForMember(dest => dest.MaternalSurName, opt => opt.MapFrom(src => src.MaternalSurName))
                .ForMember(dest => dest.Names, opt => opt.MapFrom(src => src.Names))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => $"{src.PaternalSurName} {src.MaternalSurName} {src.Names}"))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email ?? string.Empty))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.CediName, opt => opt.Ignore())
                .ForMember(dest => dest.RegionName, opt => opt.Ignore())
                .ForMember(dest => dest.ZoneCode, opt => opt.Ignore());
        }
    }
}

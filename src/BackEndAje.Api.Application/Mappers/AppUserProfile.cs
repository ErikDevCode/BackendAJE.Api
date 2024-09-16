namespace BackEndAje.Api.Application.Mappers
{
    using AutoMapper;
    using BackEndAje.Api.Application.Dtos.Users;
    using BackEndAje.Api.Domain.Entities;

    public class AppUserProfile : Profile
    {
        public AppUserProfile()
        {
            this.CreateMap<CreateUserDto, AppUser>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.RouteOrEmail, opt => opt.MapFrom(src => src.Route > 0 ? src.Route.ToString() : src.Email))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}

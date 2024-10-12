namespace BackEndAje.Api.Application.Mappers
{
    using AutoMapper;
    using BackEndAje.Api.Application.Dtos.Menu;
    using BackEndAje.Api.Domain.Entities;

    public class MenuProfile : Profile
    {
        public MenuProfile()
        {
            this.CreateMap<CreateMenuGroupDto, MenuGroup>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}

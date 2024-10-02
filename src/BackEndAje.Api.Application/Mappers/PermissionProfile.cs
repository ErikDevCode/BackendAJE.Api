namespace BackEndAje.Api.Application.Mappers
{
    using AutoMapper;
    using BackEndAje.Api.Application.Dtos.Permissions;
    using BackEndAje.Api.Application.Permissions.Queries.GetAllPermissions;
    using BackEndAje.Api.Domain.Entities;

    public class PermissionProfile : Profile
    {
        public PermissionProfile()
        {
            this.CreateMap<Permission, GetAllPermissionsResult>();

            this.CreateMap<CreatePermissionDto, Permission>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}

namespace BackEndAje.Api.Application.Mappers
{
    using AutoMapper;
    using BackEndAje.Api.Application.Dtos.Roles;
    using BackEndAje.Api.Application.Roles.Queries.GetAllRoles;
    using BackEndAje.Api.Application.Roles.Queries.GetAllRolesWithPermissions;
    using BackEndAje.Api.Application.Roles.Queries.GetPermissionsWithActionByRoleId;
    using BackEndAje.Api.Domain.Entities;

    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            this.CreateMap<UpdateRoleDto, Role>()
                .ForMember(dest => dest.RoleId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

            this.CreateMap<CreateRoleDto, Role>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

            this.CreateMap<Role, GetAllRolesResult>();

            this.CreateMap<AssignPermissionToRoleDto, RolePermission>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

            this.CreateMap<UpdateStatusRoleDto, Role>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

            this.CreateMap<RolesWithPermissions, GetAllRolesWithPermissionsResult>();
            this.CreateMap<PermissionsWithActions, GetPermissionsWithActionByRoleIdResult>();
        }
    }
}

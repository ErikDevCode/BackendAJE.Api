namespace BackEndAje.Api.Application.Roles.Commands.UpdateRole
{
    using BackEndAje.Api.Application.Dtos.Roles;
    using MediatR;

    public class UpdateRolesCommand : IRequest<Unit>
    {
        public UpdateRoleDto Role { get; set; }
    }
}

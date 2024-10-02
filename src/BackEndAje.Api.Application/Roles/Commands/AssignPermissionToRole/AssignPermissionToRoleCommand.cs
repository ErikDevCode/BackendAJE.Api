namespace BackEndAje.Api.Application.Roles.Commands.AssignPermissionToRole
{
    using BackEndAje.Api.Application.Dtos.Roles;
    using MediatR;

    public class AssignPermissionToRoleCommand : IRequest<Unit>
    {
        public AssignPermissionToRoleDto AssignPermissionToRole { get; set; }
    }
}

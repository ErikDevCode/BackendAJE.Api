namespace BackEndAje.Api.Application.Roles.Commands.DeleteRole
{
    using BackEndAje.Api.Application.Dtos.Roles;
    using MediatR;

    public class DeleteRoleCommand : IRequest<bool>
    {
        public DeleteRoleDto RoleDelete { get; set; }
    }
}

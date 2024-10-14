namespace BackEndAje.Api.Application.Roles.Commands.UpdateStatusRole
{
    using BackEndAje.Api.Application.Dtos.Roles;
    using MediatR;

    public class UpdateStatusRoleCommand : IRequest<bool>
    {
        public UpdateStatusRoleDto RoleUpdateStatus { get; set; }
    }
}

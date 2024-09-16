namespace BackEndAje.Api.Application.Roles.Commands.CreateRole
{
    using BackEndAje.Api.Application.Dtos.Roles;
    using MediatR;

    public class CreateRolesCommand : IRequest<Unit>
    {
        public CreateRoleDto Role { get; set; }
    }
}

namespace BackEndAje.Api.Application.Roles.Commands.UpdateRole
{
    using MediatR;

    public class UpdateRolesCommand : IRequest<Unit>
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public string Description { get; set; }
    }
}

namespace BackEndAje.Api.Application.Roles.Commands.CreateRole
{
    using MediatR;

    public class CreateRolesCommand : IRequest<Unit>
    {
        public string RoleName { get; set; }

        public string Description { get; set; }
    }
}

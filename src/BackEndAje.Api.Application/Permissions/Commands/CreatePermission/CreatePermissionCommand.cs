namespace BackEndAje.Api.Application.Permissions.Commands.CreatePermission
{
    using BackEndAje.Api.Application.Dtos.Permissions;
    using MediatR;

    public class CreatePermissionCommand : IRequest<Unit>
    {
        public CreatePermissionDto Permission { get; set; }
    }
}

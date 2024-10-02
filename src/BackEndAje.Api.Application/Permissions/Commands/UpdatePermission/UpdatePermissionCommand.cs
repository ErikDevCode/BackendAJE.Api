namespace BackEndAje.Api.Application.Permissions.Commands.UpdatePermission
{
    using BackEndAje.Api.Application.Dtos.Permissions;
    using MediatR;

    public class UpdatePermissionCommand : IRequest<Unit>
    {
        public UpdatePermissionDto Permission { get; set; }
    }
}

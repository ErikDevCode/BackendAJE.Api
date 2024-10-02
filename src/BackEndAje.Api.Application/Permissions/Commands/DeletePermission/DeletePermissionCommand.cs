namespace BackEndAje.Api.Application.Permissions.Commands.DeletePermission
{
    using BackEndAje.Api.Application.Dtos.Permissions;
    using MediatR;

    public class DeletePermissionCommand : IRequest<bool>
    {
        public DeletePermissionDto DeletePermission { get; set; }
    }
}

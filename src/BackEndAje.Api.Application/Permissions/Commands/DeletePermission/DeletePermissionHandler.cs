namespace BackEndAje.Api.Application.Permissions.Commands.DeletePermission
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class DeletePermissionHandler : IRequestHandler<DeletePermissionCommand, bool>
    {
        private readonly IPermissionRepository _permissionRepository;

        public DeletePermissionHandler(IPermissionRepository permissionRepository)
        {
            this._permissionRepository = permissionRepository;
        }

        public async Task<bool> Handle(DeletePermissionCommand request, CancellationToken cancellationToken)
        {
            var existingPermission = await this._permissionRepository.GetPermissionByIdAsync(request.DeletePermission.PermissionId);
            if (existingPermission == null)
            {
                throw new InvalidOperationException($"Permison con ID '{request.DeletePermission.PermissionId}' no existe.");
            }

            await this._permissionRepository.DeletePermissionAsync(existingPermission);
            return true;
        }
    }
}

namespace BackEndAje.Api.Application.Permissions.Commands.UpdatePermission
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class UpdatePermissionHandler : IRequestHandler<UpdatePermissionCommand, Unit>
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMapper _mapper;

        public UpdatePermissionHandler(IPermissionRepository permissionRepository, IMapper mapper)
        {
            this._permissionRepository = permissionRepository;
            this._mapper = mapper;
        }

        public async Task<Unit> Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
        {
            var listPermissions = await this._permissionRepository.GetAllPermissionsAsync();
            var existingPermission = listPermissions.FirstOrDefault(action => action.PermissionId == request.Permission.PermissionId);
            if (existingPermission == null)
            {
                throw new InvalidOperationException($"Permiso con ID '{request.Permission.PermissionId}' no existe.");
            }

            this._mapper.Map(request.Permission, existingPermission);
            await this._permissionRepository.UpdatePermissionAsync(existingPermission);
            return Unit.Value;
        }
    }
}

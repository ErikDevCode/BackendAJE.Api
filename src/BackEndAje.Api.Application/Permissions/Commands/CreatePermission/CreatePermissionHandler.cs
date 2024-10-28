namespace BackEndAje.Api.Application.Permissions.Commands.CreatePermission
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class CreatePermissionHandler : IRequestHandler<CreatePermissionCommand,Unit>
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMapper _mapper;

        public CreatePermissionHandler(IPermissionRepository permissionRepository, IMapper mapper)
        {
            this._permissionRepository = permissionRepository;
            this._mapper = mapper;
        }

        public async Task<Unit> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
        {
            var listPermissions = await this._permissionRepository.GetAllPermissionsAsync();
            var existingRole = listPermissions.FirstOrDefault(r => r.PermissionName.Equals(request.Permission.PermissionName, StringComparison.OrdinalIgnoreCase));

            if (existingRole != null)
            {
                throw new InvalidOperationException($"Permiso: '{request.Permission.PermissionName}' ya existe.");
            }

            var newRole = this._mapper.Map<Permission>(request.Permission);
            await this._permissionRepository.AddPermissionAsync(newRole);
            return Unit.Value;
        }
    }
}

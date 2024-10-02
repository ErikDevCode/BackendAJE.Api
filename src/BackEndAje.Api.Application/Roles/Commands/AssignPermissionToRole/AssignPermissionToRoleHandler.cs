namespace BackEndAje.Api.Application.Roles.Commands.AssignPermissionToRole
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class AssignPermissionToRoleHandler : IRequestHandler<AssignPermissionToRoleCommand, Unit>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMapper _Mapper;

        public AssignPermissionToRoleHandler(IRoleRepository roleRepository, IMapper mapper, IPermissionRepository permissionRepository)
        {
            this._roleRepository = roleRepository;
            this._Mapper = mapper;
            this._permissionRepository = permissionRepository;
        }

        public async Task<Unit> Handle(AssignPermissionToRoleCommand request, CancellationToken cancellationToken)
        {
            var existingRole = await this._roleRepository.GetRoleByIdAsync(request.AssignPermissionToRole.RoleId);
            if (existingRole == null)
            {
                throw new InvalidOperationException($"RoleId '{request.AssignPermissionToRole.RoleId}' not exists.");
            }

            var existingPermission = await this._permissionRepository.GetPermissionByIdAsync(request.AssignPermissionToRole.PermissionId);
            if (existingPermission == null)
            {
                throw new InvalidOperationException($"PermissionId '{request.AssignPermissionToRole.PermissionId}' not exists.");
            }

            var existingRolePermission = await this._roleRepository.GetRolePermissionsByIdsAsync(request.AssignPermissionToRole.RoleId, request.AssignPermissionToRole.PermissionId);
            if (existingRolePermission != null)
            {
                throw new InvalidOperationException("Ya existe la relación entre el rol y el permiso.");
            }

            this._Mapper.Map(request.AssignPermissionToRole, existingRolePermission);
            await this._roleRepository.AssignPermissionToRole(existingRolePermission!);
            return Unit.Value;
        }
    }
}

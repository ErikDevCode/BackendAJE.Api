namespace BackEndAje.Api.Application.Roles.Commands.AssignPermissionToRole
{
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class AssignPermissionToRoleHandler : IRequestHandler<AssignPermissionToRoleCommand, Unit>
    {
        private readonly IRolePermissionRepository _rolePermissionRepository;

        public AssignPermissionToRoleHandler( IRolePermissionRepository rolePermissionRepository)
        {
            this._rolePermissionRepository = rolePermissionRepository;
        }

        public async Task<Unit> Handle(AssignPermissionToRoleCommand request, CancellationToken cancellationToken)
        {
            await this._rolePermissionRepository.RolePermissionAsync(request.AssignPermissionToRole.RoleId, request.AssignPermissionToRole.PermissionId, request.AssignPermissionToRole.Status, request.AssignPermissionToRole.CreatedBy, request.AssignPermissionToRole.UpdatedBy);
            return Unit.Value;
        }
    }
}

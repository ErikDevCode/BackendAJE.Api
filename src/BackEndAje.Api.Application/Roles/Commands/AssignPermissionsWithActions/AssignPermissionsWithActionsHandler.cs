namespace BackEndAje.Api.Application.Roles.Commands.AssignPermissionsWithActions
{
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class AssignPermissionsWithActionsHandler : IRequestHandler<AssignPermissionsWithActionsCommand, Unit>
    {
        private readonly IRolePermissionRepository _rolePermissionRepository;

        public AssignPermissionsWithActionsHandler(IRolePermissionRepository rolePermissionRepository)
        {
            this._rolePermissionRepository = rolePermissionRepository;
        }

        public async Task<Unit> Handle(AssignPermissionsWithActionsCommand request, CancellationToken cancellationToken)
        {
            await this._rolePermissionRepository.AssignOrRemovePermissionWithActionAsync(request.RoleId, request.PermissionId, request.ActionId, request.Status, request.CreatedBy, request.UpdatedBy);
            return Unit.Value;
        }
    }
}


namespace BackEndAje.Api.Application.Roles.Commands.UpdateStatusRole
{
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class UpdateStatusRoleHandler : IRequestHandler<UpdateStatusRoleCommand, bool>
    {
        private readonly IRoleRepository _roleRepository;

        public UpdateStatusRoleHandler(IRoleRepository roleRepository)
        {
            this._roleRepository = roleRepository;
        }

        public async Task<bool> Handle(UpdateStatusRoleCommand request, CancellationToken cancellationToken)
        {
            var existingRole = await this._roleRepository.GetRoleByIdAsync(request.RoleUpdateStatus.RoleId);
            if (existingRole == null)
            {
                throw new KeyNotFoundException($"Rol con ID '{request.RoleUpdateStatus.RoleId}' no existe.");
            }

            existingRole.IsActive = existingRole.IsActive is false;
            await this._roleRepository.DeleteRoleAsync(existingRole);
            return true;
        }
    }
}

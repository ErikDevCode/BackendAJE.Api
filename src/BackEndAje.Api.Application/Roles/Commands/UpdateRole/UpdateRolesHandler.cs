namespace BackEndAje.Api.Application.Roles.Commands.UpdateRole
{
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class UpdateRolesHandler : IRequestHandler<UpdateRolesCommand, Unit>
    {
        private readonly IRoleRepository _roleRepository;

        public UpdateRolesHandler(IRoleRepository roleRepository)
        {
            this._roleRepository = roleRepository;
        }

        public async Task<Unit> Handle(UpdateRolesCommand request, CancellationToken cancellationToken)
        {
            var roleDto = await this._roleRepository.GetRoleByIdAsync(request.RoleId);
            if (roleDto == null)
            {
                throw new InvalidOperationException($"Role '{request.RoleName}' not exists.");
            }

            var updateRole = new Role
            {
                RoleId = request.RoleId,
                RoleName = request.RoleName,
                Description = request.Description,
            };

            await this._roleRepository.UpdateRoleAsync(updateRole);
            return Unit.Value;
        }
    }
}

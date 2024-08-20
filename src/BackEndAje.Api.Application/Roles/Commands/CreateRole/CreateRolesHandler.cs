namespace BackEndAje.Api.Application.Roles.Commands.CreateRole
{
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class CreateRolesHandler : IRequestHandler<CreateRolesCommand, Unit>
    {
        private readonly IRoleRepository _roleRepository;

        public CreateRolesHandler(IRoleRepository roleRepository)
        {
            this._roleRepository = roleRepository;
        }

        public async Task<Unit> Handle(CreateRolesCommand request, CancellationToken cancellationToken)
        {
            var listRole = await this._roleRepository.GetAllRolesAsync();
            var existingRole = listRole.FirstOrDefault(r => r.RoleName.Equals(request.RoleName, StringComparison.OrdinalIgnoreCase));

            if (existingRole != null)
            {
                throw new InvalidOperationException($"Role '{request.RoleName}' already exists.");
            }

            var newRole = new Role
            {
                RoleName = request.RoleName,
                Description = request.Description,
            };

            await this._roleRepository.AddRoleAsync(newRole);
            return Unit.Value;
        }
    }
}

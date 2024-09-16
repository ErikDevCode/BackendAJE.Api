namespace BackEndAje.Api.Application.Roles.Commands.CreateRole
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class CreateRolesHandler : IRequestHandler<CreateRolesCommand, Unit>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public CreateRolesHandler(IRoleRepository roleRepository, IMapper mapper)
        {
            this._roleRepository = roleRepository;
            this._mapper = mapper;
        }

        public async Task<Unit> Handle(CreateRolesCommand request, CancellationToken cancellationToken)
        {
            var listRole = await this._roleRepository.GetAllRolesAsync();
            var existingRole = listRole.FirstOrDefault(r => r.RoleName.Equals(request.Role.RoleName, StringComparison.OrdinalIgnoreCase));

            if (existingRole != null)
            {
                throw new InvalidOperationException($"Role '{request.Role.RoleName}' already exists.");
            }

            var newRole = this._mapper.Map<Role>(request.Role);
            await this._roleRepository.AddRoleAsync(newRole);
            return Unit.Value;
        }
    }
}

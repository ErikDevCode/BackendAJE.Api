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
            var roleExists = await this._roleRepository.RoleExistsAsync(request.Role.RoleName);
            if (roleExists)
            {
                throw new InvalidOperationException($"Role '{request.Role.RoleName}' already exists.");
            }

            var newRole = this._mapper.Map<Role>(request.Role);
            await this._roleRepository.AddRoleAsync(newRole);
            return Unit.Value;
        }
    }
}

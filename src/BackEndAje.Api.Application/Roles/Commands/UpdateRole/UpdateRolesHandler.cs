namespace BackEndAje.Api.Application.Roles.Commands.UpdateRole
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class UpdateRolesHandler : IRequestHandler<UpdateRolesCommand, Unit>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public UpdateRolesHandler(IRoleRepository roleRepository, IMapper mapper)
        {
            this._roleRepository = roleRepository;
            this._mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateRolesCommand request, CancellationToken cancellationToken)
        {
            var existingRole = await this._roleRepository.GetRoleByIdAsync(request.Role.RoleId);
            if (existingRole == null)
            {
                throw new InvalidOperationException($"Rol: '{request.Role.RoleName}' no existe.");
            }

            this._mapper.Map(request.Role, existingRole);
            await this._roleRepository.UpdateRoleAsync(existingRole);
            return Unit.Value;
        }
    }
}

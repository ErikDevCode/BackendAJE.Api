namespace BackEndAje.Api.Application.Roles.Commands.DeleteRole
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class DeleteRoleHandler : IRequestHandler<DeleteRoleCommand, bool>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public DeleteRoleHandler(IRoleRepository roleRepository, IMapper mapper)
        {
            this._roleRepository = roleRepository;
            this._mapper = mapper;
        }

        public async Task<bool> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var existingRole = await this._roleRepository.GetRoleByIdAsync(request.RoleDelete.RoleId);
            if (existingRole == null)
            {
                throw new InvalidOperationException($"RoleId '{request.RoleDelete.RoleId}' not exists.");
            }

            await this._roleRepository.DeleteRoleAsync(existingRole);
            return true;
        }
    }
}

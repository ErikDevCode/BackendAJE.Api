namespace BackEndAje.Api.Application.Roles.Commands.UpdateStatusRole
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class UpdateStatusRoleHandler : IRequestHandler<UpdateStatusRoleCommand, bool>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public UpdateStatusRoleHandler(IRoleRepository roleRepository, IMapper mapper)
        {
            this._roleRepository = roleRepository;
            this._mapper = mapper;
        }

        public async Task<bool> Handle(UpdateStatusRoleCommand request, CancellationToken cancellationToken)
        {
            var existingRole = await this._roleRepository.GetRoleByIdAsync(request.RoleUpdateStatus.RoleId);
            if (existingRole == null)
            {
                throw new InvalidOperationException($"RoleId '{request.RoleUpdateStatus.RoleId}' not exists.");
            }

            existingRole.IsActive = existingRole.IsActive is false;
            await this._roleRepository.DeleteRoleAsync(existingRole);
            return true;
        }
    }
}

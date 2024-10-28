namespace BackEndAje.Api.Application.Users.Commands.RemoveRoleToUser
{
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class RemoveRoleToUserHandler : IRequestHandler<RemoveRolesToUserCommand, Unit>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public RemoveRoleToUserHandler(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            this._userRepository = userRepository;
            this._roleRepository = roleRepository;
        }

        public async Task<Unit> Handle(RemoveRolesToUserCommand request, CancellationToken cancellationToken)
        {
            var user = await this._userRepository.GetUserWithRoleByIdAsync(request.UserId);
            if (user == null)
            {
                throw new KeyNotFoundException($"Usuario Con ID '{request.UserId}' no encontrado.");
            }

            var role = await this._roleRepository.GetRoleByIdAsync(request.RoleId);
            if (role == null)
            {
                throw new KeyNotFoundException($"Rol con ID '{request.RoleId}' no encontrado.");
            }

            var userRoles = await this._userRepository.GetUserRolesAsync(user.UserId);
            if (!userRoles.Contains(role.RoleId))
            {
                throw new InvalidOperationException($"Usuario con ID '{request.UserId}' no tiene el rol '{role.RoleName}' asignado.");
            }

            await this._userRepository.RemoveUserRoleAsync(user.UserId, role.RoleId);
            await this._userRepository.SaveChangesAsync();

            return Unit.Value;
        }
    }
}

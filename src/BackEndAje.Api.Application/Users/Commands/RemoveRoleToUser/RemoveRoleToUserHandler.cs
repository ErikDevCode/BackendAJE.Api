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
            var user = await this._userRepository.GetUserByIdAsync(request.UserId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID '{request.UserId}' not found.");
            }

            var role = await this._roleRepository.GetRoleByIdAsync(request.RoleId);
            if (role == null)
            {
                throw new KeyNotFoundException($"Role with ID '{request.RoleId}' not found.");
            }

            var userRoles = await this._userRepository.GetUserRolesAsync(user.UserId);
            if (!userRoles.Contains(role.RoleId))
            {
                throw new InvalidOperationException($"User with ID '{request.UserId}' does not have the role '{role.RoleName}' assigned.");
            }

            await this._userRepository.RemoveUserRoleAsync(user.UserId, role.RoleId);
            await this._userRepository.SaveChangesAsync();

            return Unit.Value;
        }
    }
}

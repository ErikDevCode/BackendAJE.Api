namespace BackEndAje.Api.Application.Users.Commands.AssignRolesToUser
{
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class AssignRolesToUserHandler : IRequestHandler<AssignRolesToUserCommand, AssingRolesToUserResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public AssignRolesToUserHandler(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            this._userRepository = userRepository;
            this._roleRepository = roleRepository;
        }

        public async Task<AssingRolesToUserResult> Handle(AssignRolesToUserCommand request, CancellationToken cancellationToken)
        {
            var user = await this._userRepository.GetUserByIdAsync(request.UserId);

            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID '{request.UserId}' not found.");
            }

            var currentRoles = await this._userRepository.GetUserRolesAsync(user.UserId);

            var rolesToAdd = request.Roles.Except(currentRoles, StringComparer.InvariantCultureIgnoreCase).ToList();

            var rolesToRemove = currentRoles.Except(request.Roles, StringComparer.CurrentCultureIgnoreCase).ToList();

            var assignedRoleIds = new List<int>();
            var assignedRoleNames = new List<string>();

            foreach (var role in rolesToAdd)
            {
                var roleEntity = await this._roleRepository.GetRoleByNameAsync(role);
                await this._userRepository.AddUserRoleAsync(user.UserId, roleEntity.RoleId);
                assignedRoleIds.Add(roleEntity.RoleId);
                assignedRoleNames.Add(roleEntity.RoleName);
            }

            foreach (var role in rolesToRemove)
            {
                var roleEntity = await this._roleRepository.GetRoleByNameAsync(role);
                if (roleEntity != null)
                {
                    await this._userRepository.RemoveUserRoleAsync(user.UserId, roleEntity.RoleId);
                }
            }

            await this._userRepository.SaveChangesAsync();

            return new AssingRolesToUserResult(user.UserId, assignedRoleIds, assignedRoleNames);
        }
    }
}

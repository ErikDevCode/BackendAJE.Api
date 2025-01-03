﻿namespace BackEndAje.Api.Application.Users.Commands.AssignRolesToUser
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
            var user = await this._userRepository.GetUserWithRoleByIdAsync(request.UserId);

            if (user == null)
            {
                throw new KeyNotFoundException($"Usuario con ID '{request.UserId}' no encontrado.");
            }

            var currentRoles = await this._userRepository.GetUserRolesAsync(user.UserId);

            if (currentRoles.Any(roleId => roleId == request.RoleId))
            {
                throw new InvalidOperationException($"Rol con ID '{request.RoleId}' ya esta asignado al usuario con ID '{user.UserId}'.");
            }

            var roleById = await this._roleRepository.GetRoleByIdAsync(request.RoleId);

            await this._userRepository.AddUserRoleAsync(user.UserId, request.RoleId, request.CreatedBy, request.UpdatedBy);

            await this._userRepository.SaveChangesAsync();

            return new AssingRolesToUserResult(user.UserId, request.RoleId, roleById!.RoleName);
        }
    }
}

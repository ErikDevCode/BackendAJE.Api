namespace BackEndAje.Api.Application.Users.Queries.GetUserRolesById
{
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetUserRolesByIdHandler : IRequestHandler<GetUserRolesByIdQuery, List<GetUserRolesByIdResult>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public GetUserRolesByIdHandler(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            this._userRepository = userRepository;
            this._roleRepository = roleRepository;
        }

        public async Task<List<GetUserRolesByIdResult>> Handle(GetUserRolesByIdQuery request, CancellationToken cancellationToken)
        {
            var roles = await this._userRepository.GetUserRolesAsync(request.UserId);
            if (roles == null || roles.Count == 0)
            {
                throw new KeyNotFoundException($"No hay roles encontrados para el usuario ID {request.UserId}.");
            }

            var result = new List<GetUserRolesByIdResult>();

            foreach (var roleId in roles)
            {
                var role = await this._roleRepository.GetRoleByIdAsync(roleId);

                if (role == null)
                {
                    throw new KeyNotFoundException($"Rol con ID {roleId} no encontrado.");
                }

                result.Add(new GetUserRolesByIdResult
                {
                    RoleId = role.RoleId,
                    RoleName = role.RoleName,
                });
            }

            return result;
        }
    }
}
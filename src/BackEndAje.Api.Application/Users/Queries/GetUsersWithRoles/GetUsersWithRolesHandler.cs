namespace BackEndAje.Api.Application.Users.Queries.GetUsersWithRoles
{
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetUsersWithRolesHandler : IRequestHandler<GetUsersWithRolesQuery, List<GetUsersWithRolesResult>>
    {
        private readonly IUserRepository _userRepository;

        public GetUsersWithRolesHandler(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        public async Task<List<GetUsersWithRolesResult>> Handle(GetUsersWithRolesQuery request, CancellationToken cancellationToken)
        {
            var users = await this._userRepository.GetAllUsersWithRolesAsync();

            return users.Select(user => new GetUsersWithRolesResult(
                user.UserId,
                user.Username,
                user.Email,
                user.UserRoles.Select(ur => ur.Role.RoleName).ToList())).ToList();
        }
    }
}

namespace BackEndAje.Api.Application.Users.Queries.GetUserRolesById
{
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetUserRolesByIdHandler : IRequestHandler<GetUserRolesByIdQuery, List<string>>
    {
        private readonly IUserRepository _userRepository;

        public GetUserRolesByIdHandler(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        public async Task<List<string>> Handle(GetUserRolesByIdQuery request, CancellationToken cancellationToken)
        {
            var roles = await this._userRepository.GetUserRolesAsync(request.UserId);
            if (roles == null || roles.Count == 0)
            {
                throw new KeyNotFoundException($"No roles found for user with ID {request.UserId}.");
            }

            return roles;
        }
    }
}
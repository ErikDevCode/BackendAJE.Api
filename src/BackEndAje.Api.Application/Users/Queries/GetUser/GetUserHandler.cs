namespace BackEndAje.Api.Application.Users.Queries.GetUser
{
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetUserHandler : IRequestHandler<GetUserQuery, GetUserResult>
    {
        private readonly IUserRepository _userRepository;

        public GetUserHandler(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        public async Task<GetUserResult> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await this._userRepository.GetUserByEmailAsync(request.UserEmail);

            if (user == null)
            {
                throw new KeyNotFoundException($"User with email '{request.UserEmail}' not found.");
            }

            return new GetUserResult(user.UserId, user.Username, user.Email);
        }
    }
}

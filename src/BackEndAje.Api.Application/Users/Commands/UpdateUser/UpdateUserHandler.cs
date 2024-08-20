namespace BackEndAje.Api.Application.Users.Commands.UpdateUser
{
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UpdateUserResult>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserHandler(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        public async Task<UpdateUserResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await this._userRepository.GetUserByEmailAsync(request.Email);

            if (user == null)
            {
                throw new KeyNotFoundException($"User with Email '{request.Email}' not found.");
            }

            if (!string.IsNullOrEmpty(request.Username))
            {
                user.Username = request.Username;
            }

            if (!string.IsNullOrEmpty(request.Email))
            {
                user.Email = request.Email;
            }

            user.UpdatedAt = DateTime.UtcNow;

            await this._userRepository.UpdateUserAsync(user);
            return new UpdateUserResult(user.UserId, user.Username, user.Email);
        }
    }
}

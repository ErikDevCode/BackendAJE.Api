namespace BackEndAje.Api.Application.Users.Commands.UpdateUserPassword
{
    using BackEndAje.Api.Application.Interfaces;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class UpdateUserPasswordByEmailHandler : IRequestHandler<UpdateUserPasswordByEmailCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHashingService _hashingService;

        public UpdateUserPasswordByEmailHandler(IUserRepository userRepository, IHashingService hashingService)
        {
            this._userRepository = userRepository;
            this._hashingService = hashingService;
        }

        public async Task<bool> Handle(UpdateUserPasswordByEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await this._userRepository.GetUserByEmailAsync(request.Email);

            if (user == null)
            {
                throw new KeyNotFoundException($"User with email '{request.Email}' not found.");
            }

            user.PasswordHash = this._hashingService.HashPassword(request.NewPassword);
            user.UpdatedAt = DateTime.UtcNow;

            await this._userRepository.UpdateUserAsync(user);
            await this._userRepository.SaveChangesAsync();

            return true;
        }
    }
}

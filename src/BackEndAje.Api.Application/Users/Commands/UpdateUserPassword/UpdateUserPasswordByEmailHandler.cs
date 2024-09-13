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
            var appUser = await this._userRepository.GetAppUserByEmailAsync(request.RouteOrEmail);

            if (appUser == null)
            {
                throw new KeyNotFoundException($"User with Route or email '{request.RouteOrEmail}' not found.");
            }

            appUser.PasswordHash = this._hashingService.HashPassword(request.NewPassword);
            appUser.UpdatedAt = DateTime.UtcNow;

            await this._userRepository.UpdateAppUserAsync(appUser);
            return true;
        }
    }
}

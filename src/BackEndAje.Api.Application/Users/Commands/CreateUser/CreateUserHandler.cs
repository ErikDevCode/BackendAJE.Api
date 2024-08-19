namespace BackEndAje.Api.Application.Users.Commands.CreateUser
{
    using BackEndAje.Api.Application.Interfaces;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class CreateUserHandler : IRequestHandler<CreateUserCommand, CreateUserResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHashingService _hashingService;

        public CreateUserHandler(IUserRepository userRepository, IHashingService hashingService)
        {
            this._userRepository = userRepository;
            this._hashingService = hashingService;
        }

        public async Task<CreateUserResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await this._userRepository.GetUserByEmailAsync(request.Email);
            if (existingUser != null)
            {
                throw new ArgumentException("Email already in use.");
            }

            var passwordHash = this._hashingService.HashPassword(request.Password);

            var newUser = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            await this._userRepository.AddUserAsync(newUser);
            await this._userRepository.SaveChangesAsync();

            return new CreateUserResult(newUser.UserId, newUser.Username, newUser.Email);
        }
    }
}

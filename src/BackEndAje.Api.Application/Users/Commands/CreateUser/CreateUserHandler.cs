namespace BackEndAje.Api.Application.Users.Commands.CreateUser
{
    using AutoMapper;
    using BackEndAje.Api.Application.Interfaces;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class CreateUserHandler : IRequestHandler<CreateUserCommand, CreateUserResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHashingService _hashingService;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public CreateUserHandler(IUserRepository userRepository, IHashingService hashingService, IRoleRepository roleRepository, IMapper mapper)
        {
            this._userRepository = userRepository;
            this._hashingService = hashingService;
            this._roleRepository = roleRepository;
            this._mapper = mapper;
        }

        public async Task<CreateUserResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            await this.CheckIfUserExistsAsync(request.User.Email, request.User.Route);

            var newUser = await this.CreateUserAsync(request);
            await this.CreateAppUserAsync(request, newUser);
            await this.AssignRoleToUserAsync(request, newUser);
            var role = await this._roleRepository.GetRoleByIdAsync(request.User.RoleId);

            return new CreateUserResult(
                newUser.UserId,
                $"{newUser.PaternalSurName} {newUser.MaternalSurName} {newUser.Names}",
                newUser.Email!,
                request.User.RoleId,
                role!.RoleName);
        }

        private async Task CheckIfUserExistsAsync(string? email, int? route)
        {
            var emailOrRoute = !string.IsNullOrWhiteSpace(email) ? email : route?.ToString();
            var existingUser = await this._userRepository.GetUserByEmailOrRouteAsync(emailOrRoute!);
            if (existingUser != null)
            {
                throw new ArgumentException("Email o Ruta already in use.");
            }
        }

        private async Task<User> CreateUserAsync(CreateUserCommand request)
        {
            var newUser = this._mapper.Map<User>(request.User);
            await this._userRepository.AddUserAsync(newUser);
            return newUser;
        }

        private async Task CreateAppUserAsync(CreateUserCommand request, User newUser)
        {
            var passwordHash = this._hashingService.HashPassword(request.User.Password);
            var newAppUser = this._mapper.Map<AppUser>(request.User);
            newAppUser.UserId = newUser.UserId;
            newAppUser.PasswordHash = passwordHash;
            await this._userRepository.AddAppUserAsync(newAppUser);
        }

        private async Task AssignRoleToUserAsync(CreateUserCommand request, User newUser)
        {
            if (request.User.RoleId > 0)
            {
                await this._userRepository.AddUserRoleAsync(newUser.UserId, request.User.RoleId, request.User.CreatedBy, request.User.UpdatedBy);
            }
        }
    }
}

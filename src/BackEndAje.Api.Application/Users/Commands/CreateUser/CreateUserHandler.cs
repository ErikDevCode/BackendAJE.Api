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
        private readonly IRoleRepository _roleRepository;

        public CreateUserHandler(IUserRepository userRepository, IHashingService hashingService, IRoleRepository roleRepository)
        {
            this._userRepository = userRepository;
            this._hashingService = hashingService;
            this._roleRepository = roleRepository;
        }

        public async Task<CreateUserResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var email = request.Email;
            var route = request.Route;
            var emailOrRoute = !string.IsNullOrWhiteSpace(email) ? email : route?.ToString();
            var existingUser = await this._userRepository.GetUserByEmailOrRouteAsync(emailOrRoute!);
            if (existingUser != null)
            {
                throw new ArgumentException("Email o Ruta already in use.");
            }

            var passwordHash = this._hashingService.HashPassword(request.Password);

            var newUser = new User
            {
                RegionId = request.RegionId,
                CediId = request.CediId > 0 ? request.CediId : null,
                ZoneId = request.ZoneId > 0 ? request.ZoneId : null,
                Route = request.Route > 0 ? request.Route : null,
                Code = request.Code > 0 ? request.Code : null,
                PaternalSurName = request.PaternalSurName,
                MaternalSurName = request.MaternalSurName,
                Names = request.Names,
                Email = request.Email,
                Phone = request.Phone,
                IsActive = request.IsActive,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = request.CreatedBy,
                UpdatedBy = request.UpdatedBy,
            };

            await this._userRepository.AddUserAsync(newUser);
            var newAppUser = new AppUser
            {
                UserId = newUser.UserId,
                RouteOrEmail = (newUser.Route is null ? newUser.Email : newUser.Route.ToString())!,
                PasswordHash = passwordHash,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = request.CreatedBy,
                UpdatedBy = request.UpdatedBy,
            };
            await this._userRepository.AddAppUserAsync(newAppUser);

            if (request.RoleId > 0)
            {
                await this._userRepository.AddUserRoleAsync(newUser.UserId, request.RoleId, request.CreatedBy, request.UpdatedBy);
            }

            var role = await this._roleRepository.GetRoleByIdAsync(request.RoleId);

            return new CreateUserResult(newUser.UserId,  $"{newUser.PaternalSurName} {newUser.MaternalSurName} {newUser.Names}", newUser.Email, request.RoleId, role!.RoleName);
        }
    }
}

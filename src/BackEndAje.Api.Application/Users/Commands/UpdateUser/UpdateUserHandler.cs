namespace BackEndAje.Api.Application.Users.Commands.UpdateUser
{
    using AutoMapper;
    using BackEndAje.Api.Application.Dtos.Users;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UpdateUserResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UpdateUserHandler(IUserRepository userRepository, IMapper mapper)
        {
            this._userRepository = userRepository;
            this._mapper = mapper;
        }

        public async Task<UpdateUserResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await this.GetUserByEmailOrRouteAsync(request.User.Email, request.User.Route);

            this.UpdateUserProperties(user, request.User);
            await this.UpdateAppUserAsync(user, request);


            await this._userRepository.UpdateUserAsync(user);
            return new UpdateUserResult(
                user.UserId,
                $"{user.PaternalSurName} {user.MaternalSurName} {user.Names}",
                user.Email!);
        }

        private async Task<User> GetUserByEmailOrRouteAsync(string? email, int? route)
        {
            var emailOrRoute = !string.IsNullOrWhiteSpace(email) ? email : route?.ToString();
            var user = await this._userRepository.GetUserByEmailOrRouteAsync(emailOrRoute!);
            if (user == null)
            {
                throw new KeyNotFoundException($"Usuario con Email or Ruta '{emailOrRoute}' no encontrado.");
            }

            return user;
        }

        private void UpdateUserProperties(User user, UpdateUserDto userDto)
        {
            this._mapper.Map(userDto, user);
            user.UpdatedAt = DateTime.Now;
        }

        private async Task UpdateAppUserAsync(User user, UpdateUserCommand request)
        {
            if (!string.IsNullOrEmpty(request.User.Email) && request.User.Email != user.Email)
            {
                user.Email = request.User.Email;
                var appUser = await this._userRepository.GetAppUserByRouteOrEmailAsync(user.Email);
                appUser.RouteOrEmail = request.User.Email;
                await this._userRepository.UpdateAppUserAsync(appUser);
            }

            if (request.User.Route.HasValue && request.User.Route != user.Route)
            {
                user.Route = request.User.Route;
                var appUser = await this._userRepository.GetAppUserByRouteOrEmailAsync(user.Route.ToString()!);
                appUser.RouteOrEmail = request.User.Route.ToString()!;
                await this._userRepository.UpdateAppUserAsync(appUser);
            }
        }
    }
}

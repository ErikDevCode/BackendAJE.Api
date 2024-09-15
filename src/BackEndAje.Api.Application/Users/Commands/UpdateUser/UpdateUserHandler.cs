﻿namespace BackEndAje.Api.Application.Users.Commands.UpdateUser
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
            var email = request.Email;
            var route = request.Route;
            var emailOrRoute = !string.IsNullOrWhiteSpace(email) ? email : route?.ToString();
            var user = await this._userRepository.GetUserByEmailOrRouteAsync(emailOrRoute!);

            if (user == null)
            {
                throw new KeyNotFoundException($"User with Email '{request.Email}' not found.");
            }

            user.RegionId = request.RegionId > 0 ? request.RegionId : user.RegionId;
            user.CediId = request.CediId;  // Puede ser null
            user.ZoneId = request.ZoneId;  // Puede ser null
            user.Route = request.Route;  // Puede ser null
            user.Code = request.Code;  // Puede ser null

            if (!string.IsNullOrWhiteSpace(request.PaternalSurName))
            {
                user.PaternalSurName = request.PaternalSurName;
            }

            if (!string.IsNullOrWhiteSpace(request.MaternalSurName))
            {
                user.MaternalSurName = request.MaternalSurName;
            }

            if (!string.IsNullOrWhiteSpace(request.Names))
            {
                user.Names = request.Names;
            }

            if (!string.IsNullOrWhiteSpace(request.Phone))
            {
                user.Phone = request.Phone;
            }

            user.IsActive = request.IsActive;
            user.UpdatedBy = request.UpdatedBy;
            user.UpdatedAt = DateTime.Now;

            if (!string.IsNullOrEmpty(request.Email) && request.Email != user.Email)
            {
                user.Email = request.Email;
                var appUser = await this._userRepository.GetAppUserByRouteOrEmailAsync(user.Email);
                appUser.RouteOrEmail = request.Email;
                await this._userRepository.UpdateAppUserAsync(appUser);
            }

            if (request.Route.HasValue && request.Route != user.Route)
            {
                user.Route = request.Route;
                var appUser = await this._userRepository.GetAppUserByRouteOrEmailAsync(user.Route.ToString()!);
                appUser.RouteOrEmail = request.Route.ToString()!;
                await this._userRepository.UpdateAppUserAsync(appUser);
            }

            await this._userRepository.UpdateUserAsync(user);
            return new UpdateUserResult(user.UserId, $"{user.PaternalSurName} {user.MaternalSurName} {user.Names}", user.Email);
        }
    }
}

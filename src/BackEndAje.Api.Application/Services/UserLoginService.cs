﻿namespace BackEndAje.Api.Application.Services
{
    using BackEndAje.Api.Application.Abstractions.Users;
    using BackEndAje.Api.Application.Interfaces;
    using BackEndAje.Api.Application.Users.Commands.LoginUser;
    using BackEndAje.Api.Domain.Repositories;

    public class UserLoginService : IUserLoginService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHashingService _hashingService;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public UserLoginService(IUserRepository userRepository, IHashingService hashingService, IJwtTokenGenerator jwtTokenGenerator)
        {
            this._userRepository = userRepository;
            this._hashingService = hashingService;
            this._jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<LoginUserResult> LoginAsync(string email, string password)
        {
            var user = await this._userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            var passwordValid = this._hashingService.VerifyPassword(password, user.PasswordHash);
            if (!passwordValid)
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            var roles = await this._userRepository.GetRolesByUserIdAsync(user.UserId);
            var permissions = await this._userRepository.GetPermissionsByUserIdAsync(user.UserId);
            var tokenDto = this._jwtTokenGenerator.GeneratorToken(user, roles, permissions);
            return new LoginUserResult
            {
                AccessToken = tokenDto.Token,
                TokenExpiresInSeg = tokenDto.TokenExpiresSeg,
            };
        }
    }
}

namespace BackEndAje.Api.Application.Users.Commands.LoginUser
{
    using BackEndAje.Api.Application.Abstractions.Messaging;
    using BackEndAje.Api.Application.Abstractions.Users;

    public class LoginUserHandler : ICommandHandler<LoginUserCommand, LoginUserResult>
    {
        private readonly IUserLoginService _userLoginService;

        public LoginUserHandler(IUserLoginService userLoginService)
        {
            this._userLoginService = userLoginService;
        }

        public async Task<LoginUserResult> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            return await this._userLoginService.LoginAsync(request.Email, request.Password);
        }
    }
}

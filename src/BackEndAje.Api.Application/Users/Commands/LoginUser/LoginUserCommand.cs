namespace BackEndAje.Api.Application.Users.Commands.LoginUser
{
    using BackEndAje.Api.Application.Abstractions.Messaging;

    public class LoginUserCommand : ICommand<LoginUserResult>
    {
        public string RouteOrEmail { get; set; }

        public string Password { get; set; }
    }
}

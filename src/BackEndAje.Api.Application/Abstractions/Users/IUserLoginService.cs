namespace BackEndAje.Api.Application.Abstractions.Users
{
    using BackEndAje.Api.Application.Users.Commands.LoginUser;

    public interface IUserLoginService
    {
        Task<LoginUserResult> LoginAsync(string routeOrEmail, string password);
    }
}

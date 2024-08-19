using BackEndAje.Api.Application.Users.Commands.LoginUser;

namespace BackEndAje.Api.Application.Abstractions.Users
{
    public interface IUserLoginService
    {
        Task<LoginUserResult> LoginAsync(string email, string password);
    }
}

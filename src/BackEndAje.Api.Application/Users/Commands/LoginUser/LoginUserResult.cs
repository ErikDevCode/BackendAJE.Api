namespace BackEndAje.Api.Application.Users.Commands.LoginUser
{
    public class LoginUserResult
    {
        public string AccessToken { get; set; }

        public long TokenExpiresInSeg { get; set; }
    }
}

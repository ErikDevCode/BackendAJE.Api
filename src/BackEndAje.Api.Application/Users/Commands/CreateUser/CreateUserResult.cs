namespace BackEndAje.Api.Application.Users.Commands.CreateUser
{
    public class CreateUserResult
    {
        public int UserId { get; }

        public string Username { get; }

        public string Email { get; }

        public CreateUserResult(int userId, string username, string email)
        {
            this.UserId = userId;
            this.Username = username;
            this.Email = email;
        }
    }
}

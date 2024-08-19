namespace BackEndAje.Api.Application.Users.Commands.UpdateUser
{
    public class UpdateUserResult
    {
        public int UserId { get; }

        public string Username { get; }

        public string Email { get; }

        public UpdateUserResult(int userId, string username, string email)
        {
            this.UserId = userId;
            this.Username = username;
            this.Email = email;
        }
    }
}

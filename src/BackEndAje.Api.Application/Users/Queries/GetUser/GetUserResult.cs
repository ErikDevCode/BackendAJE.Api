namespace BackEndAje.Api.Application.Users.Queries.GetUser
{
    public class GetUserResult
    {
        public int UserId { get; }

        public string Username { get; }

        public string Email { get; }

        public GetUserResult(int userId, string username, string email)
        {
            this.UserId = userId;
            this.Username = username;
            this.Email = email;
        }
    }
}

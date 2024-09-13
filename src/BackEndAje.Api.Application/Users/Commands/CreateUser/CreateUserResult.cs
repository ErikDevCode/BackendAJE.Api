namespace BackEndAje.Api.Application.Users.Commands.CreateUser
{
    public class CreateUserResult
    {
        public int UserId { get; }

        public string Username { get; }

        public string Email { get; }

        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public CreateUserResult(int userId, string username, string email, int roleId, string roleName)
        {
            this.UserId = userId;
            this.Username = username;
            this.Email = email;
            this.RoleId = roleId;
            this.RoleName = roleName;
        }
    }
}

namespace BackEndAje.Api.Application.Users.Queries.GetUsersWithRoles
{
    public class GetUsersWithRolesResult
    {
        public int UserId { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public List<string> Roles { get; set; }

        public GetUsersWithRolesResult(int userId, string username, string email, List<string> roles)
        {
            UserId = userId;
            Username = username;
            Email = email;
            Roles = roles;
        }
    }
}


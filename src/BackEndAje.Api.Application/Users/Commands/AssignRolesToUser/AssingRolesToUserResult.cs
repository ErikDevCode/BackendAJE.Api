namespace BackEndAje.Api.Application.Users.Commands.AssignRolesToUser
{
    public class AssingRolesToUserResult
    {
        public int UserId { get; set; }

        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public AssingRolesToUserResult(int userId, int roleId, string roleName)
        {
            this.UserId = userId;
            this.RoleId = roleId;
            this.RoleName = roleName;
        }
    }
}

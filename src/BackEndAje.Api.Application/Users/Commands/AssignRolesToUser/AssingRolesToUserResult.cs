namespace BackEndAje.Api.Application.Users.Commands.AssignRolesToUser
{
    public class AssingRolesToUserResult
    {
        public int UserId { get; set; }

        public List<int> RoleIds { get; set; }

        public List<string> RoleNames { get; set; }

        public AssingRolesToUserResult(int userId, List<int> roleIds, List<string> roleNames)
        {
            this.UserId = userId;
            this.RoleIds = roleIds;
            this.RoleNames = roleNames;
        }
    }
}

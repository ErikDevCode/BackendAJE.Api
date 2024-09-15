namespace BackEndAje.Api.Application.Users.Queries.GetUserByRouteOrEmail
{
    public class RoleResponse
    {
        public string RoleName { get; set; }

        public List<PermissionResponse> Permissions { get; set; } = new List<PermissionResponse>();
    }
}

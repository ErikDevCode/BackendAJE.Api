namespace BackEndAje.Api.Application.Users.Queries.GetUserByRouteOrEmail
{
    public class PermissionResponse
    {
        public string Module { get; set; }

        public ActionPermissions Actions { get; set; }
    }
}

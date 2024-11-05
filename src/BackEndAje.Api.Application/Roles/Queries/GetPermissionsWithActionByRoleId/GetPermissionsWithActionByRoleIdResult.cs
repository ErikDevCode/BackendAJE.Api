namespace BackEndAje.Api.Application.Roles.Queries.GetPermissionsWithActionByRoleId
{
    public class GetPermissionsWithActionByRoleIdResult
    {
        public int RoleId { get; set; }

        public int PermissionId { get; set; }

        public string Permission { get; set; }

        public int ActionId { get; set; }

        public string ActionName { get; set; }

        public bool Status { get; set; }
    }
}

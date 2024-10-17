namespace BackEndAje.Api.Application.Roles.Queries.GetAllRolesWithPermissions
{
    public class GetAllRolesWithPermissionsResult
    {
        public int RoleId { get; set; }

        public string Role { get; set; }

        public int PermissionId { get; set; }

        public string Permission { get; set; }

        public bool Status { get; set; }
    }
}

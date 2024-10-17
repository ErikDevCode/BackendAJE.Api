namespace BackEndAje.Api.Application.Dtos.Roles
{
    public class RolesWithPermissionsDto
    {
        public int RoleId { get; set; }

        public string Role { get; set; }

        public int PermissionId { get; set; }

        public string Permission { get; set; }

        public bool Status { get; set; }
    }
}

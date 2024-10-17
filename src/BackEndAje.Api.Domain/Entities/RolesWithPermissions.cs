namespace BackEndAje.Api.Domain.Entities
{
    public class RolesWithPermissions
    {
        public int RoleId { get; set; }

        public string Role { get; set; }

        public int PermissionId { get; set; }

        public string Permission { get; set; }

        public bool Status { get; set; }
    }
}
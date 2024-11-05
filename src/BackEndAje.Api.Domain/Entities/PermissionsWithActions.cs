namespace BackEndAje.Api.Domain.Entities
{
    public class PermissionsWithActions
    {
        public int RoleId { get; set; }

        public int PermissionId { get; set; }

        public string Permission { get; set; }

        public int ActionId { get; set; }

        public string ActionName { get; set; }

        public bool Status { get; set; }
    }
}


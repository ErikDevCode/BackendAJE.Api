namespace BackEndAje.Api.Domain.Entities
{
    public class Role : AuditableEntity
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; }
        
        public string Description { get; set; }

        public bool IsActive { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}

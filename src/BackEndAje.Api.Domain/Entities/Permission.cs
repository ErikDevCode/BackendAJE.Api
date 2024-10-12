namespace BackEndAje.Api.Domain.Entities
{
    public class Permission : AuditableEntity
    {
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
        
        public string Label { get; set; }
        
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}

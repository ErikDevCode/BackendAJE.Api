namespace BackEndAje.Api.Domain.Entities
{
    public class Role
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; }
        
        public string Description { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}

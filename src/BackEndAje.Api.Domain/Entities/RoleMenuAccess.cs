namespace BackEndAje.Api.Domain.Entities
{
    public class RoleMenuAccess
    {
        public int RoleMenuAccessId { get; set; }
        public int RolePermissionId { get; set; }
        
        public RolePermission RolePermission { get; set; }
        public int MenuItemActionId { get; set; }
        
        public MenuItemAction MenuItemAction { get; set; }
    }
}

namespace BackEndAje.Api.Domain.Entities
{
    public class MenuItemAction : AuditableEntity
    {
        public int MenuItemActionId { get; set; }
        public int MenuItemId { get; set; }
        
        public MenuItem MenuItem { get; set; }
        
        public int ActionId { get; set; }
        public Action Action { get; set; }
        
        public ICollection<RoleMenuAccess> RoleMenuAccesses { get; set; } = new List<RoleMenuAccess>();
    }
}

namespace BackEndAje.Api.Domain.Entities
{
    public class MenuItem : AuditableEntity
    {
        public int MenuItemId { get; set; }
        
        public int MenuGroupId { get; set; }
        public string Label { get; set; }
        public string Icon { get; set; }
        public string Route { get; set; }
        
        public int? ParentMenuItemId { get; set; }
        public MenuItem ParentItem { get; set; }
        public MenuGroup MenuGroup { get; set; }
        public ICollection<MenuItemAction> MenuItemActions { get; set; } = new List<MenuItemAction>();
        public ICollection<MenuItem> ChildItems { get; set; } = new List<MenuItem>();
    }
}

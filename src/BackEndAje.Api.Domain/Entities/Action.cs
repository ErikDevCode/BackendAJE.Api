namespace BackEndAje.Api.Domain.Entities
{
    public class Action : AuditableEntity
    {
        public int ActionId { get; set; }
        public string ActionName { get; set; }

        public ICollection<MenuItemAction> MenuItemActions { get; set; } = new List<MenuItemAction>();
    }
}

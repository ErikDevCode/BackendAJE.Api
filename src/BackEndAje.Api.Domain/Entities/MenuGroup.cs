namespace BackEndAje.Api.Domain.Entities
{
    public class MenuGroup : AuditableEntity
    {
        public int MenuGroupId { get; set; }
        public string GroupName { get; set; }
        public bool IsSeparator { get; set; }
        public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    }
}

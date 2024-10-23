namespace BackEndAje.Api.Domain.Entities
{
    public class MenuItemDto
    {
        public int MenuItemId { get; set; }

        public string Label { get; set; }

        public string Icon { get; set; }

        public string RouterLink { get; set; }

        public List<string> Permissions { get; set; } = new List<string>();
    }
}

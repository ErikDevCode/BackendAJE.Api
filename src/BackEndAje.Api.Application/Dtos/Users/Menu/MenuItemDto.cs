namespace BackEndAje.Api.Application.Dtos.Users.Menu
{
    public class MenuItemDto
    {
        public string Icon { get; set; }

        public string Label { get; set; }

        public string Route { get; set; }

        public List<string> Permissions { get; set; } = new List<string>();

        public List<MenuItemDto> Children { get; set; } = new List<MenuItemDto>();
    }
}

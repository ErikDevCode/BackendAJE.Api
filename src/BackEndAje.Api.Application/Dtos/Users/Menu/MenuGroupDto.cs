namespace BackEndAje.Api.Application.Dtos.Users.Menu
{
    public class MenuGroupDto
    {
        public string Group { get; set; }

        public bool Separator { get; set; }

        public List<MenuItemDto> Items { get; set; } = new List<MenuItemDto>();
    }
}

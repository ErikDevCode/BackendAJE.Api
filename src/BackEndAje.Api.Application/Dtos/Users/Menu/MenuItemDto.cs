namespace BackEndAje.Api.Application.Dtos.Users.Menu
{
    using System.Text.Json.Serialization;

    public class MenuItemDto
    {
        public string Label { get; set; }

        public string Icon { get; set; }

        public string RouterLink { get; set; }

        public List<string> Permissions { get; set; } = new List<string>();
    }
}

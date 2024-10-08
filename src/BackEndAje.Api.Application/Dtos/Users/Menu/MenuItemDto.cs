namespace BackEndAje.Api.Application.Dtos.Users.Menu
{
    using System.Text.Json.Serialization;

    public class MenuItemDto
    {
        public string Icon { get; set; }

        public string Label { get; set; }

        public string Route { get; set; }

        public List<string> Permissions { get; set; } = new List<string>();

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<MenuItemDto>? Children { get; set; }
    }
}

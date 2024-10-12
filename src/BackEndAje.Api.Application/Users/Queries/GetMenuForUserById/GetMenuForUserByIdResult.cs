namespace BackEndAje.Api.Application.Users.Queries.GetMenuForUserById
{
    using BackEndAje.Api.Application.Dtos.Users.Menu;

    public class GetMenuForUserByIdResult
    {
        public string label { get; set; }

        public List<MenuItemDto> Items { get; set; } = new List<MenuItemDto>();
    }
}

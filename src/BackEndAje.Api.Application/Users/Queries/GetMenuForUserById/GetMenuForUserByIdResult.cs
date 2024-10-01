namespace BackEndAje.Api.Application.Users.Queries.GetMenuForUserById
{
    using BackEndAje.Api.Application.Dtos.Users.Menu;

    public class GetMenuForUserByIdResult
    {
        public List<MenuGroupDto> MenuGroups { get; set; } = new List<MenuGroupDto>();
    }
}

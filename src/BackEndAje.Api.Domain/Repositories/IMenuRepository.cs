namespace BackEndAje.Api.Domain.Repositories
{
    using BackEndAje.Api.Domain.Entities;

    public interface IMenuRepository
    {
        Task<List<MenuGroup>> GetAllMenuGroupAsync();
        Task AddMenuGroupAsync(MenuGroup menuGroup); 
    }
}

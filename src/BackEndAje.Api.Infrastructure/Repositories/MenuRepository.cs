namespace BackEndAje.Api.Infrastructure.Repositories
{
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using BackEndAje.Api.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    public class MenuRepository : IMenuRepository
    {
        private readonly ApplicationDbContext _context;

        public MenuRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<List<MenuGroup>> GetAllMenuGroupAsync()
        {
            return await this._context.MenuGroups.AsNoTracking().ToListAsync();
        }

        public async Task AddMenuGroupAsync(MenuGroup menuGroup)
        {
            this._context.MenuGroups.Add(menuGroup);
            await this._context.SaveChangesAsync();
        }
    }
}

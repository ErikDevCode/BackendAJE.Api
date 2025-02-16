namespace BackEndAje.Api.Infrastructure.Repositories
{
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using BackEndAje.Api.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRoleRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<List<UserRole>> GetAllUserRolesAsync()
        {
            return await this._context.UserRoles.AsNoTracking().ToListAsync();
        }

        public async Task<List<UserRole>> GetUserRolesAsync(int userId)
        {
            return await this._context.UserRoles
                .AsNoTracking()
                .Where(ur => ur.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<UserRole>> GetUserRolesByLogisticsProviderAsync()
        {
            return await this._context.UserRoles
                .AsNoTracking()
                .Where(ur => ur.RoleId == 3) // proveedor logistico
                .ToListAsync();
        }

        public async Task<List<UserRole>> GetUserRolesByTradeAsync()
        {
            return await this._context.UserRoles
                .AsNoTracking()
                .Where(ur => ur.RoleId == 4) // trade
                .ToListAsync();
        }
    }
}

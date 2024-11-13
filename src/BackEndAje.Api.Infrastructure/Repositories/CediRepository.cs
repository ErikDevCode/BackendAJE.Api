namespace BackEndAje.Api.Infrastructure.Repositories
{
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using BackEndAje.Api.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    public class CediRepository : ICediRepository
    {
        private readonly ApplicationDbContext _context;

        public CediRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<Cedi> GetCediByCediIdAsync(int? cediId)
        {
            if (cediId == null)
            {
                return null!;
            }

            return (await this._context.Cedis.AsNoTracking().FirstOrDefaultAsync(r => r.CediId == cediId))!;
        }

        public async Task<List<Cedi>> GetCedisByRegionIdAsync(int regionId)
        {
            return await this._context.Cedis.Where(c => c.RegionId == regionId).ToListAsync();
        }

        public async Task<List<Cedi>> GetAllCedis()
        {
            return await this._context.Cedis.ToListAsync();
        }

        public async Task<List<Cedi>> GetCedisByUserIdAsync(int userId)
        {
            var isAdmin = await this._context.UserRoles
                .AnyAsync(ur => ur.UserId == userId && ur.Role.RoleName == "Administrador");

            if (isAdmin)
            {
                return await this._context.Cedis.ToListAsync();
            }

            return (await this._context.Users
                .Where(u => u.UserId == userId && u.CediId != null)
                .Select(u => u.Cedi)
                .Distinct()
                .ToListAsync())!;
        }
    }
}

namespace BackEndAje.Api.Infrastructure.Repositories
{
    using BackEndAje.Api.Application.Dtos.Const;
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
            return await this._context.Cedis.AsNoTracking().Where(c => c.RegionId == regionId).ToListAsync();
        }

        public async Task<List<Cedi>> GetAllCedis()
        {
            return await this._context.Cedis.AsNoTracking().ToListAsync();
        }

        public async Task<List<Cedi>> GetCedisByUserIdAsync(int userId)
        {
            var adminRoles = new[]
            {
                RolesConst.Administrador,
                RolesConst.Jefe,
                RolesConst.Trade,
            };

            var isAdmin = await this._context.UserRoles
                .AsNoTracking()
                .AnyAsync(ur => ur.UserId == userId && adminRoles.Contains(ur.Role.RoleName));

            if (isAdmin)
            {
                return await this._context.Cedis.ToListAsync();
            }

            return (await this._context.Users.AsNoTracking()
                .Where(u => u.UserId == userId && u.CediId != null)
                .Select(u => u.Cedi)
                .Distinct()
                .ToListAsync())!;
        }

        public async Task<Cedi?> GetCediByNameAsync(string cediName)
        {
            return await this._context.Cedis
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CediName == cediName);
        }
    }
}

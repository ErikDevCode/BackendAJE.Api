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
    }
}

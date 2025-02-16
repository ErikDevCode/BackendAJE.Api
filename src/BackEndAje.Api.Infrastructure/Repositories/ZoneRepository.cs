namespace BackEndAje.Api.Infrastructure.Repositories
{
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using BackEndAje.Api.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    public class ZoneRepository : IZoneRepository
    {
        private readonly ApplicationDbContext _context;

        public ZoneRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<Zone> GetZoneByZoneIdAsync(int? zoneId)
        {
            if (zoneId == null)
            {
                return null!;
            }

            return (await this._context.Zones.AsNoTracking().FirstOrDefaultAsync(r => r.ZoneId == zoneId))!;
        }

        public async Task<List<Zone>> GetZonesByCediIdAsync(int cediId)
        {
            return await this._context.Zones.AsNoTracking().Where(c => c.CediId == cediId).ToListAsync();
        }

        public async Task<List<Zone>> GetAllZones()
        {
            return await this._context.Zones.AsNoTracking().ToListAsync();
        }
    }
}

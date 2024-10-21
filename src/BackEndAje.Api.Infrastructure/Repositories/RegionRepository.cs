namespace BackEndAje.Api.Infrastructure.Repositories
{
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using BackEndAje.Api.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    public class RegionRepository : IRegionRepository
    {
        private readonly ApplicationDbContext _context;

        public RegionRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<Region> GetRegionByCediIdAsync(int? cediId)
        {
            var cedi = await this._context.Cedis.FirstOrDefaultAsync(r => r.CediId == cediId);
            if (cedi == null)
            {
                return null;
            }

            return (await this._context.Regions.FirstOrDefaultAsync(r => r.RegionId == cedi.RegionId))!;
        }

        public async Task<List<Region>> GetAllRegionsAsync()
        {
            return await this._context.Regions.ToListAsync();
        }
    }
}

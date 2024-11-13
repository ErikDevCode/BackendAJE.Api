namespace BackEndAje.Api.Infrastructure.Repositories
{
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using BackEndAje.Api.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    public class AssetRepository : IAssetRepository
    {
        private readonly ApplicationDbContext _context;

        public AssetRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<List<Asset>> GetAssets(int pageNumber, int pageSize, string? codeAje)
        {
            var query = this._context.Assets.AsQueryable();

            if (!string.IsNullOrEmpty(codeAje))
            {
                query = query.Where(a => a.CodeAje.Contains(codeAje));
            }

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalAssets(string? codeAje)
        {
            var query = this._context.Assets.AsQueryable();

            if (!string.IsNullOrEmpty(codeAje))
            {
                query = query.Where(a => a.CodeAje.Contains(codeAje));
            }

            return await query.CountAsync();
        }

        public async Task<Asset> GetAssetById(int assetId)
        {
            return (await this._context.Assets.AsNoTracking().FirstOrDefaultAsync(x => x.AssetId == assetId))!;
        }

        public async Task<List<Asset>> GetAssetByCodeAje(string codeAje)
        {
            return await this._context.Assets.AsNoTracking().Where(x => x.CodeAje == codeAje).ToListAsync();
        }

        public async Task AddAsset(Asset asset)
        {
            this._context.Assets.Add(asset);
            await this._context.SaveChangesAsync();
        }

        public async Task AddAssetsAsync(IEnumerable<Asset> assets)
        {
            await this._context.Assets.AddRangeAsync(assets);
            await this._context.SaveChangesAsync();
        }

        public async Task UpdateAssetAsync(Asset asset)
        {
            this._context.Entry(asset).State = EntityState.Detached;
            this._context.Assets.Update(asset);
            await this._context.SaveChangesAsync();
        }

        public async Task<Asset?> GetAssetByCodeAjeAndLogoAndAssetType(string codeAje, string logo, string? assetType)
        {
            return await this._context.Assets.AsNoTracking().FirstOrDefaultAsync(x => x.CodeAje == codeAje && x.Logo == logo && x.AssetType == assetType);
        }

        public async Task<List<Asset>> GetAssetsWithOutClient(int pageNumber, int pageSize, string? codeAje)
        {
            var query = this._context.Assets
                .Where(a => !this._context.ClientAssets.Any(ca => ca.AssetId == a.AssetId) && a.IsActive)
                .AsQueryable();

            if (!string.IsNullOrEmpty(codeAje))
            {
                query = query.Where(a => a.CodeAje.Contains(codeAje));
            }

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalAssetsWithOutClient(string? codeAje)
        {
            var query = this._context.Assets
                .Where(a => !this._context.ClientAssets.Any(ca => ca.AssetId == a.AssetId) && a.IsActive)
                .AsQueryable();

            if (!string.IsNullOrEmpty(codeAje))
            {
                query = query.Where(a => a.CodeAje.Contains(codeAje));
            }

            return await query.CountAsync();
        }
    }
}


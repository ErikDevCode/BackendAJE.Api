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

        public async Task<List<Asset>> GetAssets(int pageNumber, int pageSize)
        {
            return await this._context.Assets
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalAssets()
        {
            return await this._context.Assets.CountAsync();
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

        public async Task AddClientAsset(ClientAssets clientAssets)
        {
            this._context.ClientAssets.Add(clientAssets);
            await this._context.SaveChangesAsync();
        }

        public async Task<List<ClientAssets>> GetClientAssetsByCodeAje(string codeAje)
        {
            return await this._context.ClientAssets.AsNoTracking().Where(x => x.CodeAje == codeAje && x.IsActive).ToListAsync();
        }

        public async Task<List<ClientAssetsDto>> GetClientAssetsAsync(string? codeAje, int? clientId)
        {
            var query = this._context.ClientAssets
                .Include(ca => ca.Cedi)
                .Include(ca => ca.Client)
                .Include(ca => ca.Asset)
                .AsQueryable();

            if (!string.IsNullOrEmpty(codeAje))
            {
                query = query.Where(ca => ca.CodeAje == codeAje);
            }

            if (clientId.HasValue)
            {
                query = query.Where(ca => ca.ClientId == clientId.Value);
            }

            return await query.Select(ca => new ClientAssetsDto
            {
                ClientAssetId = ca.ClientAssetId,
                AssetId = ca.AssetId,
                CodeAje = ca.CodeAje,
                Logo = ca.Asset.Logo,
                Brand = ca.Asset.Brand,
                Model = ca.Asset.Model,
                AssetIsActive = ca.Asset.IsActive,
                InstallationDate = ca.InstallationDate,
                IsActive = ca.IsActive,
                CediId = ca.CediId,
                CediName = ca.Cedi != null ? ca.Cedi.CediName : null,
                ClientId = ca.ClientId,
                ClientCode = ca.Client.ClientCode,
                ClientName = ca.Client.CompanyName,
                Notes = ca.Notes,
                CreatedAt = ca.CreatedAt,
                UpdatedAt = ca.UpdatedAt,
            }).ToListAsync();
        }

        public async Task<ClientAssets> GetClientAssetByIdAsync(int Id)
        {
            return (await this._context.ClientAssets.AsNoTracking().FirstOrDefaultAsync(x => x.ClientAssetId == Id))!;
        }

        public async Task UpdateClientAssetsAsync(ClientAssets clientAssets)
        {
            this._context.Entry(clientAssets).State = EntityState.Detached;
            this._context.ClientAssets.Update(clientAssets);
            await this._context.SaveChangesAsync();
        }
    }
}


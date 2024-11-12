namespace BackEndAje.Api.Infrastructure.Repositories
{
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using BackEndAje.Api.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    public class ClientAssetRepository : IClientAssetRepository
    {
        private readonly ApplicationDbContext _context;

        public ClientAssetRepository(ApplicationDbContext context)
        {
            this._context = context;
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

        public async Task<List<ClientAssetsDto>> GetClientAssetsAsync(int? pageNumber, int? pageSize, string? codeAje, int? clientId, int? userId)
        {
            var currentMonthPeriod = DateTime.Now.ToString("yyyyMM");
            var query = this._context.ClientAssets
                .Include(ca => ca.Cedi)
                .Include(ca => ca.Client)
                .ThenInclude(cli => cli.Seller)
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

            if (userId.HasValue)
            {
                query = query.Where(ca => ca.Client.Seller!.UserId == userId.Value);
            }

            var clientAssets = query.Select(ca => new ClientAssetsDto
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
                UserId = ca.Client.UserId,
                Seller = (ca.Client.Seller != null
                    ? $"{ca.Client.Seller.Names} {ca.Client.Seller.PaternalSurName} {ca.Client.Seller.MaternalSurName}"
                    : null)!,
                Route = ca.Client.Route,
                ClientCode = ca.Client.ClientCode,
                ClientName = ca.Client.CompanyName,
                Notes = ca.Notes,
                IsCensus = this._context.CensusAnswer
                    .Any(cs => cs.ClientId == ca.ClientId
                               && cs.AssetId == ca.AssetId
                               && cs.MonthPeriod == currentMonthPeriod),
                CreatedAt = ca.CreatedAt,
                UpdatedAt = ca.UpdatedAt,
            });

            if (pageNumber.HasValue && pageSize.HasValue && pageNumber > 0 && pageSize > 0)
            {
                clientAssets = clientAssets
                    .Skip((pageNumber.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);
            }

            return await clientAssets.ToListAsync();
        }

        public async Task<int> GetTotalClientAssets(string? codeAje, int? clientId)
        {
            var query = this._context.ClientAssets.AsQueryable();

            if (!string.IsNullOrEmpty(codeAje))
            {
                query = query.Where(ca => ca.CodeAje == codeAje);
            }

            if (clientId.HasValue)
            {
                query = query.Where(ca => ca.ClientId == clientId.Value);
            }

            return await query.CountAsync();
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

        public async Task AddTraceabilityRecordAsync(ClientAssetsTrace clientAssetsTrace)
        {
            this._context.ClientAssetsTrace.Add(clientAssetsTrace);
            await this._context.SaveChangesAsync();
        }

        public async Task<List<ClientAssetsTrace>> GetClientAssetTracesByAssetId(int pageNumber, int pageSize, int assetId)
        {
            return await this._context.ClientAssetsTrace
                .Where(cat => cat.AssetId == assetId)
                .Select(cat => new ClientAssetsTrace
                {
                    ClientAssetTraceId = cat.ClientAssetTraceId,
                    ClientAssetId = cat.ClientAssetId,
                    PreviousClientId = cat.PreviousClientId,
                    NewClientId = cat.NewClientId,
                    AssetId = cat.AssetId,
                    CodeAje = cat.CodeAje,
                    ChangeReason = cat.ChangeReason,
                    IsActive = cat.IsActive,
                    CreatedAt = cat.CreatedAt,
                    CreatedBy = cat.CreatedBy,
                })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalClientAssetsTrace(int? assetId)
        {
            var query = this._context.ClientAssetsTrace.AsQueryable();

            if (assetId.HasValue)
            {
                query = query.Where(ca => ca.AssetId == assetId.Value);
            }

            return await query.CountAsync();
        }
    }
}
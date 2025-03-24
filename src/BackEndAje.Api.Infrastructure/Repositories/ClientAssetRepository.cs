using BackEndAje.Api.Application.Dtos.Users;

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

        public async Task AddClientListAsset(List<ClientAssets> clientAssetsList)
        {
            // Agrega todos los registros de la lista en la base
            this._context.ClientAssets.AddRange(clientAssetsList);

            // Aplica cambios
            await this._context.SaveChangesAsync();
        }

        public async Task<List<ClientAssets>> GetClientAssetsByCodeAje(string codeAje)
        {
            return await this._context.ClientAssets.AsNoTracking().Where(x => x.CodeAje == codeAje && x.IsActive == true).ToListAsync();
        }

        public async Task<List<ClientAssetsDto>> GetClientAssetsAsync(
            int? pageNumber,
            int? pageSize,
            string? codeAje,
            int? clientId,
            int? userId,
            int? cediId,
            int? regionId,
            int? route,
            int? clientCode)
        {
            var currentMonthPeriod = DateTime.Now.ToString("yyyyMM");

            
            var userRole = this._context.UserRoles.Where(x => x.UserId == userId)
                .Include(x => x.Role)
                .Select(x => new { x.Role.RoleName, x.User.CediId })
                .FirstOrDefault();

            var query = this._context.ClientAssets
                .Include(ca => ca.Cedi)
                .Include(ca => ca.Client)
                    .ThenInclude(cli => cli.Seller)
                .Include(ca => ca.Asset)
                .AsQueryable();
            if(userRole != null)
            {
                if (userRole.RoleName.Equals("Proveedor Lógistico"))
                {
                    userId = null;
                }
            }
           

            // Aplicar filtros
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
                if (userRole.RoleName.Equals("Supervisor"))
                {
                    query = query.Where(ca => ca.Client.Seller!.CediId == userRole.CediId);
                }
                else
                {
                    query = query.Where(ca => ca.Client.Seller!.UserId == userId.Value);
                }
            }

            if (cediId.HasValue)
            {
                query = query.Where(ca => ca.CediId == cediId.Value);
            }

            if (regionId.HasValue)
            {
                query = query.Where(ca => ca.Cedi.RegionId == regionId.Value);
            }

            if (route.HasValue)
            {
                query = query.Where(ca => ca.Client.Route == route.Value);
            }

            if (clientCode.HasValue)
            {
                query = query.Where(ca => ca.Client.ClientCode == clientCode.Value);
            }

            // Proyección a DTO
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

            // Paginación
            if (pageNumber.HasValue && pageSize.HasValue && pageNumber > 0 && pageSize > 0)
            {
                clientAssets = clientAssets
                    .OrderBy(ca => ca.CreatedAt)
                    .Skip((pageNumber.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);
            }

            return await clientAssets.ToListAsync();
        }


        public async Task<int> GetTotalClientAssets(string? codeAje, int? clientId, int? userId, int? cediId, int? regionId, int? route, int? clientCode)
        {
            var userRole = this._context.UserRoles.Where(x => x.UserId == userId)
                .Include(x => x.Role)
                .Select(x => new { x.Role.RoleName, x.User.CediId })
                .FirstOrDefault();
            var query = this._context.ClientAssets.AsQueryable();

            if(userRole != null)
            {
                if (userRole.RoleName.Equals("Proveedor Lógistico"))
                {
                    userId = null;
                }
            }

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
                if (userRole.RoleName.Equals("Supervisor"))
                {
                    query = query.Where(ca => ca.Client.Seller!.CediId == userRole.CediId);
                }
                else
                {
                    query = query.Where(ca => ca.Client.Seller!.UserId == userId.Value);
                }
            }

            if (cediId.HasValue)
            {
                query = query.Where(ca => ca.CediId == cediId.Value);
            }

            if (regionId.HasValue)
            {
                query = query.Where(ca => ca.Cedi.RegionId == regionId.Value);
            }

            if (route.HasValue)
            {
                query = query.Where(ca => ca.Client.Route == route.Value);
            }

            if (clientCode.HasValue)
            {
                query = query.Where(ca => ca.Client.ClientCode == clientCode.Value);
            }

            return await query.CountAsync();
        }

        public async Task<ClientAssets> GetClientAssetByIdAsync(int Id)
        {
            return (await this._context.ClientAssets.AsNoTracking().FirstOrDefaultAsync(x => x.ClientAssetId == Id))!;
        }

        public async Task<ClientAssets> GetClientAssetByClientIdAndAssetIdAndIsNotActivateAsync(int clientId, int assetId)
        {
            return (await this._context.ClientAssets.AsNoTracking().FirstOrDefaultAsync(x => x.ClientId == clientId && x.AssetId == assetId && x.IsActive == null))!;
        }

        public async Task UpdateClientAssetsAsync(ClientAssets clientAssets)
        {
            this._context.Entry(clientAssets).State = EntityState.Detached;
            this._context.ClientAssets.Update(clientAssets);
            await this._context.SaveChangesAsync();
        }

        public async Task UpdateClientAssetsListAsync(List<ClientAssets> clientAssetsList)
        {
            foreach (var clientAsset in clientAssetsList)
            {
                this._context.Entry(clientAsset).State = EntityState.Detached;
            }

            // Actualiza los registros
            this._context.ClientAssets.UpdateRange(clientAssetsList);

            // Guarda los cambios en la base de datos
            await this._context.SaveChangesAsync();
        }

        public async Task DeleteClientAssetAsync(ClientAssets clientAssets)
        {
            this._context.ClientAssets.Remove(clientAssets);
            await this._context.SaveChangesAsync();
        }

        public async Task AddTraceabilityRecordAsync(ClientAssetsTrace clientAssetsTrace)
        {
            this._context.ClientAssetsTrace.Add(clientAssetsTrace);
            await this._context.SaveChangesAsync();
        }

        public async Task AddTraceabilityRecordListAsync(List<ClientAssetsTrace> clientAssetsTraceList)
        {
            this._context.ClientAssetsTrace.AddRange(clientAssetsTraceList);
            await this._context.SaveChangesAsync();
        }

        public async Task<List<ClientAssetsTrace>> GetClientAssetTracesByAssetId(int pageNumber, int pageSize, int? assetId)
        {
            return await this._context.ClientAssetsTrace
                .Where(cat => !assetId.HasValue || cat.AssetId == assetId)
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

        public async Task<ClientAssets> GetClientAssetPendingApprovalByClientIdAndAssetIdAsync(int clientId, int assetId)
        {
            return (await this._context.ClientAssets.AsNoTracking().FirstOrDefaultAsync(x => x.ClientId == clientId && x.AssetId == assetId))!;
        }

        public async Task<ClientAssets> GetClientAssetByClientIdAndAssetId(int clientId, int assetId)
        {
            return (await this._context.ClientAssets.AsNoTracking().FirstOrDefaultAsync(x => x.ClientId == clientId && x.AssetId == assetId))!;
        }

        public async Task<List<ClientAssets>> GetClientAssetByAssetId(int assetId)
        {
            return await this._context.ClientAssets.Where(x => x.AssetId == assetId).AsNoTracking().ToListAsync();
        }
    }
}
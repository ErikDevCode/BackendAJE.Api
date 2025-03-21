namespace BackEndAje.Api.Infrastructure.Repositories
{
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using BackEndAje.Api.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    public class ClientRepository : IClientRepository
    {
        private readonly ApplicationDbContext _context;

        public ClientRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task AddClient(Client client)
        {
            this._context.Clients.Add(client);
            await this._context.SaveChangesAsync();
        }

        public async Task AddClientsAsync(IEnumerable<Client> clients)
        {
            await this._context.Clients.AddRangeAsync(clients);
            await this._context.SaveChangesAsync();
        }

        public async Task<Client?> GetClientByDocumentNumber(string documentNumber)
        {
            return await this._context.Clients.AsNoTracking()
                .FirstOrDefaultAsync(x => x.DocumentNumber == documentNumber);
        }

        public async Task<Client?> GetClientByClientCode(int clientCode, int cediId)
        {
            var client = await this._context.Clients
                .AsNoTracking()
                .Where(c => c.ClientCode == clientCode)
                .Include(c => c.DocumentType)
                .Include(c => c.PaymentMethod)
                .Include(c => c.District)
                .FirstOrDefaultAsync();

            if (client?.Route == null)
            {
                return null;
            }

            var seller = await this._context.Users
                .Include(u => u.Cedi)
                .Include(u => u.Zone)
                .SingleOrDefaultAsync(u => u.Route == client.Route && u.CediId == cediId);

            if (seller == null)
            {
                return null;
            }

            client.Seller = seller;

            return client;
        }

        public async Task<List<Client?>> GetListClientByClientCode(int clientCode)
        {
            var clients = await this._context.Clients
                .AsNoTracking()
                .Where(c => c.ClientCode == clientCode)
                .Include(c => c.DocumentType)
                .Include(c => c.PaymentMethod)
                .Include(c => c.District)
                .ToListAsync();

            if (clients.Count == 0)
            {
                return new List<Client>();
            }

            foreach (var client in clients)
            {
                if (client.Route == null)
                {
                    continue;
                }

                var seller = await this._context.Users
                    .Include(u => u.Cedi)
                    .Include(u => u.Zone)
                    .SingleOrDefaultAsync(u => u.Route == client.Route);

                if (seller != null)
                {
                    client.Seller = seller;
                }
            }

            return clients;
        }

        public async Task<Client?> GetClientByClientCodeAndRoute(int clientCode, int cediId, int? route)
        {
            var client = await this._context.Clients
                .AsNoTracking()
                .Where(c => c.ClientCode == clientCode && (!route.HasValue || c.Route == route))
                .Include(c => c.DocumentType)
                .Include(c => c.PaymentMethod)
                .Include(c => c.District)
                .FirstOrDefaultAsync();

            if (client == null)
            {
                return null;
            }

            var seller = await this._context.Users
                .Include(u => u.Cedi)
                .Include(u => u.Zone)
                .SingleOrDefaultAsync(u => (!client.Route.HasValue || u.Route == client.Route) && u.CediId == cediId);

            if (seller == null)
            {
                return null;
            }

            client.Seller = seller;

            return client;
        }

        public async Task<ClientWithAssetDto?> GetClientByClientCodeAndRouteWithAsset(int clientCode, int cediId, int? route)
        {
            var client = await this._context.Clients
                .AsNoTracking()
                .Where(c => c.ClientCode == clientCode && (!route.HasValue || c.Route == route))
                .Include(c => c.DocumentType)
                .Include(c => c.PaymentMethod)
                .Include(c => c.District)
                .Include(c => c.ClientAssets)
                    .ThenInclude(clientAssets => clientAssets.Cedi)
                .Include(c => c.ClientAssets)
                    .ThenInclude(clientAssets => clientAssets.Asset)
                .FirstOrDefaultAsync();

            if (client == null)
            {
                return null;
            }

            var seller = await this._context.Users
                .Include(u => u.Cedi)
                .Include(u => u.Zone)
                .SingleOrDefaultAsync(u => (!client.Route.HasValue || u.Route == client.Route) && u.CediId == cediId);

            if (seller == null)
            {
                return null;
            }

            client.Seller = seller;

            var clientWithAssetDto = new ClientWithAssetDto
            {
                ClientId = client.ClientId,
                ClientCode = client.ClientCode,
                CompanyName = client.CompanyName,
                DocumentTypeId = client.DocumentTypeId,
                DocumentType = client.DocumentType,
                DocumentNumber = client.DocumentNumber,
                Email = client.Email,
                EffectiveDate = client.EffectiveDate,
                PaymentMethodId = client.PaymentMethodId,
                PaymentMethod = client.PaymentMethod,
                UserId = seller?.UserId ?? 0,
                Route = client.Route,
                Seller = seller,
                Phone = client.Phone,
                Address = client.Address,
                DistrictId = client.DistrictId,
                District = client.District,
                CoordX = client.CoordX,
                CoordY = client.CoordY,
                Segmentation = client.Segmentation,
                IsActive = client.IsActive,

                // Mapea cada entidad ClientAssets a ClientAssetForClientDto
                ClientAssets = client.ClientAssets
                    .Where(clientAsset => clientAsset.IsActive == true)
                    .Select(clientAsset => new ClientAssets
                {
                    ClientAssetId = clientAsset.ClientAssetId,
                    CediId = clientAsset.CediId,
                    InstallationDate = clientAsset.InstallationDate,
                    ClientId = clientAsset.ClientId,
                    AssetId = clientAsset.AssetId,
                    CodeAje = clientAsset.CodeAje,
                    Notes = clientAsset.Notes,
                    IsActive = clientAsset.IsActive,
                    Cedi = clientAsset.Cedi,
                    Asset = clientAsset.Asset,
                    CreatedAt = clientAsset.CreatedAt,
                    UpdatedAt = clientAsset.UpdatedAt,
                    CreatedBy = clientAsset.CreatedBy,
                    UpdatedBy = clientAsset.UpdatedBy,
                }).ToList(),
            };

            return clientWithAssetDto;
        }

        public async Task<List<Client>> GetClients(int pageNumber, int pageSize, string? filtro)
        {
            var query = this._context.Clients
                .AsNoTracking()
                .Include(c => c.DocumentType)
                .Include(c => c.PaymentMethod)
                .Include(c => c.District)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filtro))
            {
                query = query.Where(c =>
                    EF.Functions.Like(c.ClientCode!, $"%{filtro}%") ||
                    EF.Functions.Like(c.CompanyName, $"%{filtro}%") ||
                    EF.Functions.Like(c.DocumentNumber!, $"%{filtro}%"));
            }

            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            var clients = await query.ToListAsync();

            foreach (var client in clients.Where(c => c.Route != null))
            {
                client.Seller = await this._context.Users
                    .Include(u => u.Cedi)
                    .Include(u => u.Zone)
                    .SingleOrDefaultAsync(u => u.Route == client.Route);
            }

            return clients;
        }

        public async Task<List<Client>> GetClientsList()
        {
            return await this._context.Clients
                .Include(c => c.Seller)
                .Include(d => d.DocumentType)
                .Include(p => p.PaymentMethod)
                .Include(di => di.District)
                .AsNoTracking().ToListAsync();
        }

        public async Task<List<Client>> GetClientsOnlyList()
        {
            return await this._context.Clients
                .AsNoTracking().ToListAsync();
        }

        public async Task<int> GetTotalClients(string? filtro)
        {
            var query = this._context.Clients
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(filtro))
            {
                query = query.Where(c =>
                    c.ClientCode.ToString().Contains(filtro) ||
                    c.CompanyName.Contains(filtro) ||
                    c.DocumentNumber.Contains(filtro));
            }

            return await query.CountAsync();
        }

        public async Task<Client?> GetClientById(int clientId)
        {
            return await this._context.Clients.AsNoTracking().FirstOrDefaultAsync(x => x.ClientId == clientId);
        }

        public async Task UpdateClientAsync(Client client)
        {
            var existingClient = await this._context.Clients.FindAsync(client.ClientId);

            if (existingClient != null)
            {
                this._context.Entry(existingClient).CurrentValues.SetValues(client);
            }
            else
            {
                this._context.Clients.Attach(client);
                this._context.Entry(client).State = EntityState.Modified;
            }

            await this._context.SaveChangesAsync();
        }

        public async Task<List<Client>> GetClientsByClientCodesAsync(IEnumerable<int> clientCodes)
        {
            return await this._context.Clients
                .Where(c => clientCodes.Contains(c.ClientCode))
                .ToListAsync();
        }

        public async Task UpdateClientsAsync(IEnumerable<Client> clients)
        {
            this._context.Clients.UpdateRange(clients);
            await this._context.SaveChangesAsync();
        }

        public void Detach<TEntity>(TEntity entity) where TEntity : class
        {
            this._context.Entry(entity).State = EntityState.Detached;
        }
    }
}

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

        public async Task<List<Client>> GetClients(int pageNumber, int pageSize)
        {
            var clients = await this._context.Clients
                .Include(c => c.DocumentType)
                .Include(c => c.PaymentMethod)
                .Include(c => c.District)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            foreach (var client in clients.Where(c => c.Route != null))
            {
                client.Seller = await this._context.Users
                    .Include(u => u.Cedi)
                    .Include(u => u.Zone)
                    .SingleOrDefaultAsync(u => u.Route == client.Route);
            }

            return clients;
        }

        public async Task<int> GetTotalClients()
        {
            return await this._context.Clients.CountAsync();
        }

        public async Task<Client?> GetClientById(int clientId)
        {
            return await this._context.Clients.AsNoTracking().FirstOrDefaultAsync(x => x.ClientId == clientId);
        }

        public async Task UpdateClientAsync(Client client)
        {
            this._context.Entry(client).State = EntityState.Detached;
            this._context.Clients.Update(client);
            await this._context.SaveChangesAsync();
        }
    }
}

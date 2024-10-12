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

        public async Task<Client?> GetClientByDocumentNumber(string documentNumber)
        {
            return await this._context.Clients.AsNoTracking()
                .FirstOrDefaultAsync(x => x.DocumentNumber == documentNumber);
        }

        public async Task<List<Client>> GetClients(int pageNumber, int pageSize)
        {
            return await this._context.Clients
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).Where(x => x.IsActive)
                .ToListAsync();
        }

        public async Task<int> GetTotalClients()
        {
            return await this._context.Clients.Where(x => x.IsActive).CountAsync();
        }
    }
}

namespace BackEndAje.Api.Domain.Repositories
{
    using BackEndAje.Api.Domain.Entities;
    
    public interface IClientRepository
    {
        Task AddClient(Client client);

        Task AddClientsAsync(IEnumerable<Client> clients);

        Task<Client?> GetClientByDocumentNumber(string documentNumber);
        
        Task<Client?> GetClientByClientCode(int clientCode, int cediId);

        Task<List<Client>> GetClients(int pageNumber, int pageSize);
        
        Task<int> GetTotalClients();
        
        Task<Client?> GetClientById(int clientId);
        
        Task UpdateClientAsync(Client client);
    }
}

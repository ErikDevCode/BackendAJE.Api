namespace BackEndAje.Api.Domain.Repositories
{
    using BackEndAje.Api.Domain.Entities;
    
    public interface IClientRepository
    {
        Task AddClient(Client client);

        Task AddClientsAsync(IEnumerable<Client> clients);

        Task<Client?> GetClientByDocumentNumber(string documentNumber);
        
        Task<Client?> GetClientByClientCode(int clientCode, int cediId);
        Task<List<Client?>> GetListClientByClientCode(int clientCode);
        
        Task<Client?> GetClientByClientCodeAndRoute(int clientCode, int cediId, int? route);
        
        Task<ClientWithAssetDto?> GetClientByClientCodeAndRouteWithAsset(int clientCode, int cediId, int? route);

        Task<List<Client>> GetClients(int pageNumber, int pageSize, string? filtro, int userId);
        
        Task<List<Client>> GetClientsList();
        
        Task<List<Client>> GetClientsOnlyList();
        
        Task<int> GetTotalClients(string? filtro, int userId);
        
        Task<Client?> GetClientById(int clientId);
        
        Task UpdateClientAsync(Client client);
        
        Task<List<Client>> GetClientsByClientCodesAsync(IEnumerable<int> clientCodes);
        
        Task UpdateClientsAsync(IEnumerable<Client> clients);
        
        public void Detach<TEntity>(TEntity entity) where TEntity : class;
    }
}

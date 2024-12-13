namespace BackEndAje.Api.Domain.Repositories
{
    using BackEndAje.Api.Domain.Entities;
    
    public interface IClientRepository
    {
        Task AddClient(Client client);

        Task AddClientsAsync(IEnumerable<Client> clients);

        Task<Client?> GetClientByDocumentNumber(string documentNumber);
        
        Task<Client?> GetClientByClientCode(int clientCode, int cediId);
        
        Task<Client?> GetClientByClientCodeAndRoute(int clientCode, int cediId, int? route);
        
        Task<ClientWithAssetDto?> GetClientByClientCodeAndRouteWithAsset(int clientCode, int cediId, int? route);

        Task<List<Client>> GetClients(int pageNumber, int pageSize, string? filtro);
        
        Task<int> GetTotalClients(string? filtro);
        
        Task<Client?> GetClientById(int clientId);
        
        Task UpdateClientAsync(Client client);
    }
}

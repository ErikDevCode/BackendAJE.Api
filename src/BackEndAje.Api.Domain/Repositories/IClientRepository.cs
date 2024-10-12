namespace BackEndAje.Api.Domain.Repositories
{
    using BackEndAje.Api.Domain.Entities;
    
    public interface IClientRepository
    {
        Task AddClient(Client client);

        Task<Client?> GetClientByDocumentNumber(string documentNumber);

        Task<List<Client>> GetClients(int pageNumber, int pageSize);
        
        Task<int> GetTotalClients();
    }
}

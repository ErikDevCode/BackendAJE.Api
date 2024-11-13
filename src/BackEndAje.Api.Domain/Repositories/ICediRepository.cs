namespace BackEndAje.Api.Domain.Repositories
{
    using BackEndAje.Api.Domain.Entities;
    
    public interface ICediRepository
    {
        Task<Cedi> GetCediByCediIdAsync(int? cediId);
        
        Task<List<Cedi>> GetCedisByRegionIdAsync(int regionId);
        
        Task<List<Cedi>> GetAllCedis();
        
        Task<List<Cedi>> GetCedisByUserIdAsync(int userId);
    }
}

namespace BackEndAje.Api.Domain.Repositories
{
    using BackEndAje.Api.Domain.Entities;
    
    public interface IRegionRepository
    {
        Task<Region> GetRegionByCediIdAsync(int? cediId);
        Task<List<Region>> GetAllRegionsAsync();
    }
}

namespace BackEndAje.Api.Domain.Repositories
{
    using BackEndAje.Api.Domain.Entities;
    
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllPermissionsAsync();
    }
}

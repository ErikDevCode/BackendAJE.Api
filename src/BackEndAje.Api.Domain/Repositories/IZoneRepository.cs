using BackEndAje.Api.Domain.Entities;

namespace BackEndAje.Api.Domain.Repositories
{
    public interface IZoneRepository
    {
        Task<Zone> GetZoneByZoneIdAsync(int? zoneId);
        
        Task<List<Zone>> GetZonesByCediIdAsync(int cediId);
    }
}

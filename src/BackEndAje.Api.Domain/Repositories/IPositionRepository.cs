using BackEndAje.Api.Domain.Entities;

namespace BackEndAje.Api.Domain.Repositories
{
    public interface IPositionRepository
    {
        Task<List<Position>> GetAllPaginatePositionsAsync(int pageNumber, int pageSize);
        Task<int> GetTotalPositionsCountAsync();

        Task<Position?> GetPositionByIdAsync(int positionId);
        
        Task AddPositionAsync(Position position);

        Task UpdatePositionAsync(Position position);
        
        Task<bool> PositionExistsAsync(string roleName);
        
    }
}

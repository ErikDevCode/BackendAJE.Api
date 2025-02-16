namespace BackEndAje.Api.Infrastructure.Repositories
{
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using BackEndAje.Api.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    public class PositionRepository : IPositionRepository
    {
        private readonly ApplicationDbContext _context;

        public PositionRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<List<Position>> GetAllPaginatePositionsAsync(int pageNumber, int pageSize)
        {
            return await this._context.Positions
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalPositionsCountAsync()
        {
            return await this._context.Positions.CountAsync();
        }

        public async Task<Position?> GetPositionByIdAsync(int positionId)
        {
            return await this._context.Positions.AsNoTracking().FirstOrDefaultAsync(r => r.PositionId == positionId);
        }

        public async Task AddPositionAsync(Position position)
        {
            this._context.Positions.Add(position);
            await this._context.SaveChangesAsync();
        }

        public async Task UpdatePositionAsync(Position position)
        {
            this._context.Entry(position).State = EntityState.Detached;
            this._context.Positions.Update(position);
            await this._context.SaveChangesAsync();
        }

        public async Task<bool> PositionExistsAsync(string positionName)
        {
            return await this._context.Positions.AnyAsync(r => r.PositionName.Equals(positionName, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}

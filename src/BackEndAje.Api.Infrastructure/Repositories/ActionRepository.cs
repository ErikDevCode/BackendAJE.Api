namespace BackEndAje.Api.Infrastructure.Repositories
{
    using BackEndAje.Api.Domain.Repositories;
    using BackEndAje.Api.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;
    using Action = BackEndAje.Api.Domain.Entities.Action;

    public class ActionRepository : IActionRepository
    {
        private readonly ApplicationDbContext _context;

        public ActionRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<List<Action>> GetAllActionsAsync()
        {
            return await this._context.Actions.ToListAsync();
        }

        public async Task AddActionAsync(Action action)
        {
            this._context.Actions.Add(action);
            await this._context.SaveChangesAsync();
        }

        public async Task UpdateActionAsync(Action action)
        {
            this._context.Entry(action).State = EntityState.Detached;
            this._context.Actions.Update(action);
            await this._context.SaveChangesAsync();
        }
    }
}

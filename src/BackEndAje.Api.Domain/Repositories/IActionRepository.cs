namespace BackEndAje.Api.Domain.Repositories
{
    using Action = BackEndAje.Api.Domain.Entities.Action;

    public interface IActionRepository
    {
        Task<List<Action>> GetAllActionsAsync();
        
        Task<Action?> GetActionByIdAsync(int actionId);
        
        Task AddActionAsync(Action role);
        
        Task UpdateActionAsync(Action role);
        Task DeleteActionAsync(Action action);
    }
}

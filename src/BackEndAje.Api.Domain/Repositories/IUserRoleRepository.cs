namespace BackEndAje.Api.Domain.Repositories
{
    using BackEndAje.Api.Domain.Entities;
    
    public interface IUserRoleRepository
    {
        Task<List<UserRole>> GetAllUserRolesAsync();

        Task<List<UserRole>> GetUserRolesAsync(int userId);
        
        Task<List<UserRole>> GetUserRolesByLogisticsProviderAsync();
        
        Task<List<UserRole>> GetUserRolesByTradeAsync();
    }
}

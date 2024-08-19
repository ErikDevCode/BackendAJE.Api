namespace BackEndAje.Api.Domain.Repositories
{
    using BackEndAje.Api.Domain.Entities;

    public interface IUserRepository
    {
        Task<User> GetUserByEmailAsync(string email);

        Task<IEnumerable<string>> GetRolesByUserIdAsync(int userId);

        Task<IEnumerable<string>> GetPermissionsByUserIdAsync(int userId);
        
        Task<User?> GetUserByIdAsync(int userId);

        Task<List<string>> GetUserRolesAsync(int userId);
        
        Task AddUserRoleAsync(int userId, int roleId);
        
        Task RemoveUserRoleAsync(int userId, int roleId);
        
        Task<List<User>> GetAllUsersWithRolesAsync();

        Task AddUserAsync(User user);

        Task UpdateUserAsync(User user);
        Task SaveChangesAsync();
    }
}

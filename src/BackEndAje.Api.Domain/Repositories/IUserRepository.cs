namespace BackEndAje.Api.Domain.Repositories
{
    using BackEndAje.Api.Domain.Entities;

    public interface IUserRepository
    {
        Task<User> GetUserByEmailOrRouteAsync(string codeRouteOrEmail);
        Task<AppUser> GetAppUserByEmailAsync(string routeOrEmail);

        Task<IEnumerable<string>> GetRolesByUserIdAsync(int userId);

        Task<IEnumerable<Role>> GetRolesWithPermissionsByUserIdAsync(int userId);

        Task<IEnumerable<Permission>> GetPermissionsByUserIdAsync(int userId);
        
        Task<User?> GetUserByIdAsync(int userId);

        Task<List<int>> GetUserRolesAsync(int userId);
        
        Task AddUserRoleAsync(int userId, int roleId);

        Task RemoveUserRoleAsync(int userId, int roleId);
        
        Task<List<User>> GetAllUsersWithRolesAsync();

        Task AddUserAsync(User user);
        Task AddAppUserAsync(AppUser appUser);

        Task UpdateUserAsync(User user);

        Task UpdateAppUserAsync(AppUser appUser);
        Task SaveChangesAsync();
    }
}

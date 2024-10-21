namespace BackEndAje.Api.Domain.Repositories
{
    using BackEndAje.Api.Domain.Entities;

    public interface IUserRepository
    {
        Task<User?> GetUserByEmailOrRouteAsync(string codeRouteOrEmail);
        Task<AppUser> GetAppUserByRouteOrEmailAsync(string routeOrEmail);

        Task<IEnumerable<string>> GetRolesByUserIdAsync(int userId);

        Task<IEnumerable<Role>> GetRolesWithPermissionsByUserIdAsync(int userId);

        Task<IEnumerable<Permission>> GetPermissionsByUserIdAsync(int userId);
        
        Task<User?> GetUserWithRoleByIdAsync(int userId);

        Task<List<int>> GetUserRolesAsync(int userId);
        
        Task AddUserRoleAsync(int userId, int roleId, int createdBy, int updatedBy);

        Task RemoveUserRoleAsync(int userId, int roleId);
        
        Task<List<User>> GetAllUsersWithRolesAsync();

        Task AddUserAsync(User user);
        Task AddAppUserAsync(AppUser appUser);

        Task UpdateUserAsync(User user);

        Task UpdateAppUserAsync(AppUser appUser);
        Task SaveChangesAsync();

        Task<List<MenuItem>> GetMenuForUserByIdAsync(int userId);
        
        Task<User?> GetUserByRouteAsync(int? route);

        Task<List<User>> GetAllUsers(int pageNumber, int pageSize);
        Task<int> GetTotalUsers();
        
        Task<User?> GetUserByIdAsync(int userId);
        
        Task AddUsersAsync(IEnumerable<User> users);
        
        Task AddAppUsersAsync(IEnumerable<AppUser> appUsers);
        
        Task<UserRole> GetUserRoleByUserIdAsync(int userId);
        
        Task<User?> GetUserByDocumentNumberAsync(string? documentNumber);

        Task<List<User>> GetUsersByParamAsync(string param);
    }
}

namespace BackEndAje.Api.Domain.Repositories
{
    using BackEndAje.Api.Domain.Entities;
    public interface IRoleRepository
    {
        Task<Role> GetRoleByNameAsync(string roleName);

        Task<List<Role>> GetAllRolesAsync();
        
        Task AddRoleAsync(Role role);

        Task UpdateRoleAsync(Role role);
        Task SaveChangesAsync();
    }
}

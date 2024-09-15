namespace BackEndAje.Api.Domain.Repositories
{
    using BackEndAje.Api.Domain.Entities;
    public interface IRoleRepository
    {
        Task<List<Role>> GetAllRolesAsync();

        Task<Role?> GetRoleByIdAsync(int roleId);
        
        Task AddRoleAsync(Role role);

        Task UpdateRoleAsync(Role role);
        Task SaveChangesAsync();
    }
}

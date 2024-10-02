namespace BackEndAje.Api.Domain.Repositories
{
    using BackEndAje.Api.Domain.Entities;
    public interface IRoleRepository
    {
        Task<List<Role>> GetAllRolesAsync();

        Task<Role?> GetRoleByIdAsync(int roleId);
        
        Task AddRoleAsync(Role role);

        Task UpdateRoleAsync(Role role);

        Task<RolePermission?> GetRolePermissionsByIdsAsync(int roleId, int permissionId);
        
        Task AssignPermissionToRole(RolePermission rolePermission);

        Task DeleteRoleAsync(Role role);
        Task SaveChangesAsync();
    }
}

namespace BackEndAje.Api.Domain.Repositories
{
    using BackEndAje.Api.Domain.Entities;
    
    public interface IPermissionRepository
    {
        Task<List<Permission>> GetAllPermissionsAsync();
        
        Task<Permission?> GetPermissionByIdAsync(int permissionId);
        Task AddPermissionAsync(Permission permission); 
        Task UpdatePermissionAsync(Permission permission);
        
        Task DeletePermissionAsync(Permission permission);
    }
}

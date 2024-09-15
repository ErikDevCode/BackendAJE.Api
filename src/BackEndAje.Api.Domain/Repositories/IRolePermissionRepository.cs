namespace BackEndAje.Api.Domain.Repositories
{
    using BackEndAje.Api.Domain.Entities;
    
    public interface IRolePermissionRepository
    {
        Task<List<RolePermission>> GetAllRolePermissionsAsync();
    }
}

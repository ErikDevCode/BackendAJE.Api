namespace BackEndAje.Api.Domain.Repositories
{
    using BackEndAje.Api.Domain.Entities;
    
    public interface IRolePermissionRepository
    {
        Task<List<RolePermission>> GetAllRolePermissionsAsync();

        Task RolePermissionAsync(int roleId, int permissionId, bool status, int createdBy, int updatedBy);

        Task AssignOrRemovePermissionWithActionAsync(int roleId, int permissionId, int actionId, bool status,
            int createdBy, int updatedBy);
    }
}

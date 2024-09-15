namespace BackEndAje.Api.Domain.Repositories
{
    using BackEndAje.Api.Domain.Entities;
    
    public interface IPermissionRepository
    {
        Task<List<Permission>> GetAllPermissionsAsync();
    }
}

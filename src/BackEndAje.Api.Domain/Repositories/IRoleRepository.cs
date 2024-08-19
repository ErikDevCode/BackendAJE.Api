namespace BackEndAje.Api.Domain.Repositories
{
    using BackEndAje.Api.Domain.Entities;
    public interface IRoleRepository
    {
        Task<Role> GetRoleByNameAsync(string roleName);
    }
}

namespace BackEndAje.Api.Infrastructure.Repositories
{
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using BackEndAje.Api.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    public class PermissionRepository : IPermissionRepository
    {
        private readonly ApplicationDbContext _context;

        public PermissionRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<List<Permission>> GetAllPermissionsAsync()
        {
            return await this._context.Permissions.AsNoTracking().ToListAsync();
        }

        public async Task<Permission?> GetPermissionByIdAsync(int permissionId)
        {
            return await this._context.Permissions.AsNoTracking().FirstOrDefaultAsync(r => r.PermissionId == permissionId);
        }

        public async Task AddPermissionAsync(Permission permission)
        {
            this._context.Permissions.Add(permission);
            await this._context.SaveChangesAsync();
        }

        public async Task UpdatePermissionAsync(Permission permission)
        {
            this._context.Entry(permission).State = EntityState.Detached;
            this._context.Permissions.Update(permission);
            await this._context.SaveChangesAsync();
        }

        public async Task DeletePermissionAsync(Permission permission)
        {
            this._context.Permissions.Remove(permission);
            await this._context.SaveChangesAsync();
        }
    }
}

﻿namespace BackEndAje.Api.Infrastructure.Repositories
{
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using BackEndAje.Api.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<List<Role>> GetAllRolesAsync(int pageNumber, int pageSize)
        {
            return await this._context.Roles
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).Where(x => x.IsActive)
                .ToListAsync();
        }

        public async Task<int> GetTotalRolesCountAsync()
        {
            return await this._context.Roles.Where(x => x.IsActive).CountAsync();
        }

        public async Task<Role?> GetRoleByIdAsync(int roleId)
        {
            return await this._context.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.RoleId == roleId);
        }

        public async Task AddRoleAsync(Role role)
        {
            this._context.Roles.Add(role);
            await this._context.SaveChangesAsync();
        }

        public async Task UpdateRoleAsync(Role role)
        {
            this._context.Entry(role).State = EntityState.Detached;
            this._context.Roles.Update(role);
            await this._context.SaveChangesAsync();
        }

        public async Task<RolePermission?> GetRolePermissionsByIdsAsync(int roleId, int permissionId)
        {
            return await this._context.RolePermissions.AsNoTracking().FirstOrDefaultAsync(r => r.RoleId == roleId && r.PermissionId == permissionId);
        }

        public async Task AssignPermissionToRole(RolePermission rolePermission)
        {
            this._context.RolePermissions.Add(rolePermission);
            await this._context.SaveChangesAsync();
        }

        public async Task DeleteRoleAsync(Role role)
        {
            this._context.Entry(role).State = EntityState.Detached;
            this._context.Roles.Update(role);
            await this._context.SaveChangesAsync();
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await this._context.Roles.AnyAsync(r => r.RoleName.Equals(roleName, StringComparison.OrdinalIgnoreCase));
        }

        public async Task SaveChangesAsync()
        {
            await this._context.SaveChangesAsync();
        }
    }
}

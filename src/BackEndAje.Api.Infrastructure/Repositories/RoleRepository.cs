namespace BackEndAje.Api.Infrastructure.Repositories
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

        public async Task<List<Role>> GetAllPaginateRolesAsync(int pageNumber, int pageSize)
        {
            return await this._context.Roles
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalRolesCountAsync()
        {
            return await this._context.Roles.CountAsync();
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
            return await this._context.Roles.AnyAsync(r => r.RoleName.ToLower() == roleName.ToLower());
        }

        public async Task SaveChangesAsync()
        {
            await this._context.SaveChangesAsync();
        }

        public async Task<List<RolesWithPermissions>> GetRoleWithPermissionsAsync(int? roleId = null)
        {
            var rolesWithPermissions = await (
                from r in this._context.Roles
                from p in this._context.Permissions
                join rp in this._context.RolePermissions
                    on new { r.RoleId, p.PermissionId } equals new { rp.RoleId, rp.PermissionId } into rolePermissions
                from rp in rolePermissions.DefaultIfEmpty()
                where !roleId.HasValue || r.RoleId == roleId.Value
                select new RolesWithPermissions
                {
                    RoleId = r.RoleId,
                    Role = r.RoleName,
                    PermissionId = p.PermissionId,
                    Permission = p.Label,
                    Status = rp != null,
                })
            .OrderBy(rp => rp.Role).ThenBy(rp => rp.Permission).ToListAsync();

            return rolesWithPermissions;
        }

        public async Task<List<Role>> GetAllRolesAsync()
        {
            return await this._context.Roles
                .ToListAsync();
        }

        public async Task<List<PermissionsWithActions>> GetPermissionsWithActionByRoleIdAsync(int roleId)
        {
            var permissionsWithActions = await (
                    from p in this._context.Permissions
                    from a in this._context.Actions
                    join mi in this._context.MenuItems on p.Label equals mi.Label into menuItems
                    from mi in menuItems.DefaultIfEmpty()
                    join mia in this._context.MenuItemActions on new { mi.MenuItemId, a.ActionId } equals new { mia.MenuItemId, mia.ActionId } into menuItemActions
                    from mia in menuItemActions.DefaultIfEmpty()
                    join rp in this._context.RolePermissions on new { PermissionId = p.PermissionId, RoleId = roleId } equals new { rp.PermissionId, rp.RoleId } into rolePermissions
                    from rp in rolePermissions.DefaultIfEmpty()
                    join rma in this._context.RoleMenuAccess on new { RolePermissionId = rp.RolePermissionId, MenuItemActionId = mia.MenuItemActionId } equals new { rma.RolePermissionId, rma.MenuItemActionId } into roleMenuAccess
                    from rma in roleMenuAccess.DefaultIfEmpty()
                    select new PermissionsWithActions
                    {
                        RoleId = roleId,
                        PermissionId = p.PermissionId,
                        Permission = p.Label,
                        ActionId = a.ActionId,
                        ActionName = a.ActionName,
                        Status = rma != null,
                    }
                )
                .OrderBy(pa => pa.PermissionId)
                .ThenBy(pa => pa.ActionId)
                .ToListAsync();

            return permissionsWithActions;
        }
    }
}

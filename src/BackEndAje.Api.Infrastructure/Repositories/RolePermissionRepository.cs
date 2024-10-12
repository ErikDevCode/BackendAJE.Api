namespace BackEndAje.Api.Infrastructure.Repositories
{
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using BackEndAje.Api.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    public class RolePermissionRepository : IRolePermissionRepository
    {
        private readonly ApplicationDbContext _context;

        public RolePermissionRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<List<RolePermission>> GetAllRolePermissionsAsync()
        {
            return await this._context.RolePermissions.ToListAsync();
        }

        public async Task RolePermissionAsync(int roleId, int permissionId, bool status, int createdBy, int updatedBy)
        {
            // Paso 1: Verificar si el RolePermission ya existe
            var rolePermission = await this._context.RolePermissions
                .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);

            if (rolePermission != null)
            {
                // Paso 2a: Si existe, actualizar su estado
                rolePermission.Status = status;
                rolePermission.UpdatedAt = DateTime.Now;
                rolePermission.UpdatedBy = updatedBy;
                this._context.RolePermissions.Update(rolePermission);
                await this._context.SaveChangesAsync();
                return;
            }

                // Paso 2b: Si no existe, crear un nuevo RolePermission
            rolePermission = new RolePermission
                {
                    RoleId = roleId,
                    PermissionId = permissionId,
                    Status = status,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = createdBy,
                    UpdatedBy = createdBy,
                };
            await this._context.RolePermissions.AddAsync(rolePermission);
            await this._context.SaveChangesAsync();

            // Paso 3: Buscar el label de la tabla Permissions
            var permission = await this._context.Permissions
                .FirstOrDefaultAsync(p => p.PermissionId == permissionId);

            // Paso 4: Buscar el MenuItemId correspondiente al label
            var menuItem = await this._context.MenuItems
                .FirstOrDefaultAsync(mi => mi.Label == permission!.Label);

            // Paso 5: Obtener la lista de MenuItemActionId correspondientes al MenuItemId
            var menuItemActions = await this._context.MenuItemActions
                .Where(mia => mia.MenuItemId == menuItem!.MenuItemId)
                .ToListAsync();

            // Paso 6: Insertar en RoleMenuAccess los registros con RolePermissionId y MenuItemActionId
            foreach (var roleMenuAccess in menuItemActions.Select(menuItemAction => new RoleMenuAccess
                     {
                         RolePermissionId = rolePermission.RolePermissionId,
                         MenuItemActionId = menuItemAction.MenuItemActionId,
                     }))
            {
                await this._context.RoleMenuAccess.AddAsync(roleMenuAccess);
            }

            await this._context.SaveChangesAsync();
        }
    }
}

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

        public async Task AssignOrRemovePermissionWithActionAsync(int roleId, int permissionId, int actionId, bool status, int createdBy, int updatedBy)
        {
           // Paso 1: Verificar si el RolePermission ya existe para el permiso y acción específicos
            var rolePermission = await this._context.RolePermissions
                .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);

            if (status)
            {
                if (rolePermission == null)
                {
                    // Paso 2a: Si no existe, crear un nuevo RolePermission
                    rolePermission = new RolePermission
                    {
                        RoleId = roleId,
                        PermissionId = permissionId,
                        Status = true,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        CreatedBy = createdBy,
                        UpdatedBy = createdBy,
                    };
                    await this._context.RolePermissions.AddAsync(rolePermission);
                }
                else
                {
                    // Paso 2b: Si existe, asegurarnos de que está activo
                    rolePermission.Status = true;
                    rolePermission.UpdatedAt = DateTime.Now;
                    rolePermission.UpdatedBy = updatedBy;
                    this._context.RolePermissions.Update(rolePermission);
                }

                await this._context.SaveChangesAsync();

                // Paso 3: Verificar si ya existe la relación en RoleMenuAccess para este permiso y acción
                var roleMenuAccess = await this._context.RoleMenuAccess
                    .FirstOrDefaultAsync(rma => rma.RolePermissionId == rolePermission.RolePermissionId && rma.MenuItemActionId == actionId);

                if (roleMenuAccess == null)
                {
                    // Paso 4a: Crear la relación en RoleMenuAccess si no existe
                    roleMenuAccess = new RoleMenuAccess
                    {
                        RolePermissionId = rolePermission.RolePermissionId,
                        MenuItemActionId = actionId,
                    };
                    await this._context.RoleMenuAccess.AddAsync(roleMenuAccess);
                }
                else
                {
                    this._context.RoleMenuAccess.Update(roleMenuAccess);
                }
            }
            else
            {
                if (rolePermission != null)
                {
                    // Paso 5: Eliminar la relación en RoleMenuAccess si existe y status es false
                    var roleMenuAccess = await this._context.RoleMenuAccess
                        .FirstOrDefaultAsync(rma => rma.RolePermissionId == rolePermission.RolePermissionId && rma.MenuItemActionId == actionId);

                    if (roleMenuAccess != null)
                    {
                        this._context.RoleMenuAccess.Remove(roleMenuAccess);
                    }

                    // Verificar si el RolePermission debería quedar desactivado
                    rolePermission.Status = false;
                    rolePermission.UpdatedAt = DateTime.Now;
                    rolePermission.UpdatedBy = updatedBy;
                    this._context.RolePermissions.Update(rolePermission);
                }
            }

            // Guardar todos los cambios
            await this._context.SaveChangesAsync();
        }
    }
}

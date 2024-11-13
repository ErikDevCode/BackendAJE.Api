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

                // Nuevo paso: Verificar la existencia de MenuItemActionId

                // Paso 2c: Buscar en MenuItems usando el campo Label en Permissions para obtener el MenuItemId
                var permissionLabel = await this._context.Permissions
                    .Where(p => p.PermissionId == permissionId)
                    .Select(p => p.Label)
                    .FirstOrDefaultAsync();

                if (permissionLabel == null)
                {
                    throw new Exception("No se encontró el permiso correspondiente al PermissionId especificado.");
                }

                var menuItem = await this._context.MenuItems
                    .FirstOrDefaultAsync(mi => mi.Label == permissionLabel);

                if (menuItem == null)
                {
                    throw new Exception("No se encontró el MenuItem correspondiente al permiso.");
                }

                // Paso 2d: Buscar o crear el MenuItemActionId usando el MenuItemId y el ActionId
                var menuItemAction = await this._context.MenuItemActions
                    .FirstOrDefaultAsync(mia => mia.MenuItemId == menuItem.MenuItemId && mia.ActionId == actionId);

                if (menuItemAction == null)
                {
                    // Crear nuevo MenuItemAction si no existe
                    menuItemAction = new MenuItemAction
                    {
                        MenuItemId = menuItem.MenuItemId,
                        ActionId = actionId,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        CreatedBy = createdBy,
                        UpdatedBy = createdBy,
                    };
                    await this._context.MenuItemActions.AddAsync(menuItemAction);
                    await this._context.SaveChangesAsync();
                }

                // Paso 3: Verificar si ya existe la relación en RoleMenuAccess para este permiso y acción
                var roleMenuAccess = await this._context.RoleMenuAccess
                    .FirstOrDefaultAsync(rma => rma.RolePermissionId == rolePermission.RolePermissionId && rma.MenuItemActionId == menuItemAction.MenuItemActionId);

                if (roleMenuAccess == null)
                {
                    // Paso 4a: Crear la relación en RoleMenuAccess si no existe
                    roleMenuAccess = new RoleMenuAccess
                    {
                        RolePermissionId = rolePermission.RolePermissionId,
                        MenuItemActionId = menuItemAction.MenuItemActionId,
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
                    // Paso 5a: Obtener el Label del permiso
                    var permissionLabel = await this._context.Permissions
                        .Where(p => p.PermissionId == permissionId)
                        .Select(p => p.Label)
                        .FirstOrDefaultAsync();

                    if (permissionLabel == null)
                    {
                        throw new Exception("No se encontró el permiso correspondiente al PermissionId especificado.");
                    }

                    // Paso 5b: Usar el Label para obtener el MenuItemId en MenuItems
                    var menuItemId = await this._context.MenuItems
                        .Where(mi => mi.Label == permissionLabel)
                        .Select(mi => mi.MenuItemId)
                        .FirstOrDefaultAsync();

                    if (menuItemId == 0) // Ajusta según el valor por defecto de MenuItemId si no se encuentra
                    {
                        throw new Exception("No se encontró el MenuItem correspondiente al permiso.");
                    }

                    // Paso 5c: Usar el MenuItemId y ActionId para obtener el MenuItemActionId en MenuItemActions
                    var menuItemAction = await this._context.MenuItemActions
                        .FirstOrDefaultAsync(mia => mia.MenuItemId == menuItemId && mia.ActionId == actionId);

                    if (menuItemAction != null)
                    {
                        // Paso 5d: Eliminar la relación en RoleMenuAccess si existe
                        var roleMenuAccess = await this._context.RoleMenuAccess
                            .FirstOrDefaultAsync(rma => rma.RolePermissionId == rolePermission.RolePermissionId && rma.MenuItemActionId == menuItemAction.MenuItemActionId);

                        if (roleMenuAccess != null)
                        {
                            this._context.RoleMenuAccess.Remove(roleMenuAccess);
                        }
                    }
                }
            }

            // Guardar todos los cambios
            await this._context.SaveChangesAsync();
        }
    }
}

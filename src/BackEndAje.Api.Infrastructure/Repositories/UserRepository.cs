namespace BackEndAje.Api.Infrastructure.Repositories
{
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using BackEndAje.Api.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<User?> GetUserByEmailOrRouteAsync(string codeRouteOrEmail)
        {
            if (string.IsNullOrEmpty(codeRouteOrEmail))
            {
                return null!;
            }

            if (codeRouteOrEmail.Contains("@"))
            {
                return await this._context.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Email == codeRouteOrEmail);
            }
            else if (int.TryParse(codeRouteOrEmail, out var route))
            {
                return await this._context.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Route == route);
            }

            return null;
        }

        public async Task<AppUser> GetAppUserByRouteOrEmailAsync(string routeOrEmail)
        {
            return (await this._context.AppUsers.AsNoTracking().SingleOrDefaultAsync(u => u.RouteOrEmail == routeOrEmail)) !;
        }

        public async Task<AppUser> GetAppUserByUserId(int userId)
        {
            return (await this._context.AppUsers.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == userId)) !;
        }

        public async Task<IEnumerable<string>> GetRolesByUserIdAsync(int userId)
        {
            var roles = await this._context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Join(
                    this._context.Roles,
                    ur => ur.RoleId,
                    r => r.RoleId,
                    (ur, r) => r.RoleName)
                .AsNoTracking()
                .ToListAsync();

            return roles;
        }

        public async Task<IEnumerable<Role>> GetRolesWithPermissionsByUserIdAsync(int userId)
        {
            var rolesWithPermissions = await this._context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Include(ur => ur.Role)
                .ThenInclude(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .Select(ur => ur.Role)
                .ToListAsync();

            return rolesWithPermissions;
        }

        public async Task<IEnumerable<Permission>> GetPermissionsByUserIdAsync(int userId)
        {
            var permissions = await this._context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Join(
                    this._context.RolePermissions,
                    ur => ur.RoleId,
                    rp => rp.RoleId,
                    (ur, rp) => rp.PermissionId)
                .Join(
                    this._context.Permissions,
                    rp => rp,
                    p => p.PermissionId,
                    (rp, p) => new Permission()
                    {
                        PermissionId = p.PermissionId,
                        PermissionName = p.PermissionName,
                    })
                .ToListAsync();

            return permissions;
        }

        public async Task<User?> GetUserWithRoleByIdAsync(int userId)
        {
            return await this._context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<List<int>> GetUserRolesAsync(int userId)
        {
            return await this._context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.Role.RoleId)
                .ToListAsync();
        }

        public async Task AddUserRoleAsync(int userId, int roleId, int createdBy, int updatedBy)
        {
            var userRole = new UserRole
            {
                UserId = userId,
                RoleId = roleId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = createdBy,
                UpdatedBy = updatedBy,
            };

            this._context.UserRoles.Add(userRole);
            await this._context.SaveChangesAsync();
        }

        public async Task RemoveUserRoleAsync(int userId, int roleId)
        {
            var userRole = await this._context.UserRoles
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

            if (userRole != null)
            {
                this._context.UserRoles.Remove(userRole);
                await this._context.SaveChangesAsync();
            }
        }

        public async Task<List<User>> GetAllUsersWithRolesAsync()
        {
            return await this._context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .ToListAsync();
        }

        public async Task AddUserAsync(User user)
        {
            this._context.Users.Add(user);
            await this._context.SaveChangesAsync();
        }

        public async Task AddAppUserAsync(AppUser appUser)
        {
            this._context.AppUsers.Add(appUser);
            await this._context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            this._context.Entry(user).State = EntityState.Detached;
            this._context.Users.Update(user);
            await this._context.SaveChangesAsync();
        }

        public async Task UpdateAppUserAsync(AppUser appUser)
        {
            this._context.Entry(appUser).State = EntityState.Detached;
            this._context.AppUsers.Update(appUser);
            await this._context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await this._context.SaveChangesAsync();
        }

        public async Task<List<MenuItemDto>> GetMenuForUserByIdAsync(int userId)
        {
            var roleIds = await this._context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.RoleId)
                .ToListAsync();

            var accessibleActions = await this._context.RoleMenuAccess
                .Where(rma => roleIds.Contains(rma.RolePermission.RoleId) && rma.RolePermission.Status)
                .Select(rma => rma.MenuItemActionId)
                .ToListAsync();

            var menuItems = await this._context.MenuItems
                .Where(mi => mi.MenuItemActions.Any(mia => accessibleActions.Contains(mia.MenuItemActionId)))
                .Select(mi => new MenuItemDto
                {
                    MenuItemId = mi.MenuItemId,
                    Label = mi.Label,
                    Icon = mi.Icon,
                    RouterLink = mi.Route,
                    Permissions = mi.MenuItemActions
                        .Where(mia => accessibleActions.Contains(mia.MenuItemActionId))
                        .Select(mia => mia.Action.ActionName)
                        .Distinct()
                        .ToList(),
                })
                .AsNoTracking()
                .ToListAsync();

            return menuItems;
        }

        public async Task<User?> GetUserByRouteAsync(int? route)
        {
            return await this._context.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Route == route);
        }

        public async Task<List<User>> GetAllUsers(int pageNumber, int pageSize, string? filtro)
        {
            var query = this._context.Users
                .Include(u => u.Cedi)
                .ThenInclude(c => c!.Region)
                .Include(u => u.Zone)
                .Include(u => u.Position)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(filtro))
            {
                query = query.Where(u =>
                    EF.Functions.Like(u.DocumentNumber!, $"%{filtro}%") ||
                    EF.Functions.Like(u.Phone, $"%{filtro}%") ||
                    EF.Functions.Like(u.Email!, $"%{filtro}%"));
            }

            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return await query.ToListAsync();
        }

        public async Task<int> GetTotalUsers()
        {
            return await this._context.Users.CountAsync();
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            var users = await this._context.Users
                .Include(c => c.Cedi)
                .ThenInclude(c => c!.Region)
                .Include(c => c.Zone)
                .Include(c => c.Position)
                .Include(c => c.UserRoles)
                .ThenInclude(c => c.Role)
                .FirstOrDefaultAsync(u => u.UserId == userId);
            return users;
        }

        public async Task AddUsersAsync(IEnumerable<User> users)
        {
            await this._context.Users.AddRangeAsync(users);
            await this._context.SaveChangesAsync();
        }

        public async Task AddAppUsersAsync(IEnumerable<AppUser> appUsers)
        {
            await this._context.AppUsers.AddRangeAsync(appUsers);
            await this._context.SaveChangesAsync();
        }

        public async Task<UserRole> GetUserRoleByUserIdAsync(int userId)
        {
            return (await this._context.UserRoles.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId))!;
        }

        public async Task<User?> GetUserByDocumentNumberAsync(string? documentNumber)
        {
            return await this._context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.DocumentNumber == documentNumber);
        }

        public async Task<List<User>> GetUsersByParamAsync(string param)
        {
            IQueryable<User> query = this._context.Users;

            if (!string.IsNullOrWhiteSpace(param))
            {
                query = query.Where(u => u.Names.ToLower().Contains(param) ||
                                          u.PaternalSurName.ToLower().Contains(param) ||
                                          u.MaternalSurName.ToLower().Contains(param) ||
                                          (u.Route != null && u.Route.ToString()!.Contains(param)));
            }

            return await query
                .Where(u => u.Route != null)
                .OrderBy(u => u.Names)
                .Take(20)
                .ToListAsync();
        }

        public async Task<List<User>> GetSupervisorByCediId(int cediId)
        {
            const int roleId = 5;

            var supervisors = await this._context.Users
                .Join(
                    this._context.UserRoles,
                    user => user.UserId,
                    userRole => userRole.UserId,
                    (user, userRole) => new { user, userRole }
                )
                .Where(u => u.userRole.RoleId == roleId && u.user.CediId == cediId)
                .Select(u => u.user)
                .ToListAsync();

            return supervisors;
        }
    }
}

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

        public async Task<User> GetUserByEmailOrRouteAsync(string codeRouteOrEmail)
        {
            if (string.IsNullOrEmpty(codeRouteOrEmail))
            {
                return null!;
            }

            if (codeRouteOrEmail.Contains("@"))
            {
                return (await this._context.Users.SingleOrDefaultAsync(u => u.Email == codeRouteOrEmail))!;
            }
            else if (int.TryParse(codeRouteOrEmail, out var route))
            {
                return (await this._context.Users.SingleOrDefaultAsync(u => u.Route == route))!;
            }

            return null!;
        }

        public async Task<AppUser> GetAppUserByRouteOrEmailAsync(string routeOrEmail)
        {
            return (await this._context.AppUsers.SingleOrDefaultAsync(u => u.RouteOrEmail == routeOrEmail)) !;
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
                        Action = p.Action,
                    })
                .ToListAsync();

            return permissions;
        }

        public async Task<User?> GetUserByIdAsync(int userId)
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
            this._context.Users.Update(user);
            await this._context.SaveChangesAsync();
        }

        public async Task UpdateAppUserAsync(AppUser appUser)
        {
            this._context.AppUsers.Update(appUser);
            await this._context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await this._context.SaveChangesAsync();
        }
    }
}

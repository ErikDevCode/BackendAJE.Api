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

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return (await this._context.Users.SingleOrDefaultAsync(u => u.Email == email)) !;
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

        public async Task<IEnumerable<string>> GetPermissionsByUserIdAsync(int userId)
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
                    (rp, p) => p.PermissionName)
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

        public async Task<List<string>> GetUserRolesAsync(int userId)
        {
            return await this._context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.Role.RoleName)
                .ToListAsync();
        }

        public async Task AddUserRoleAsync(int userId, int roleId)
        {
            var userRole = new UserRole
            {
                UserId = userId,
                RoleId = roleId,
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

        public async Task UpdateUserAsync(User user)
        {
            this._context.Users.Update(user);
        }

        public async Task SaveChangesAsync()
        {
            await this._context.SaveChangesAsync();
        }
    }
}

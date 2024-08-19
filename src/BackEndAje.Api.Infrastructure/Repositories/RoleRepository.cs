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

        public async Task<Role> GetRoleByNameAsync(string roleName)
        {
            return (await this._context.Roles
                .FirstOrDefaultAsync(r => r.RoleName == roleName))!;
        }
    }
}

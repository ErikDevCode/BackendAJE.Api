namespace BackEndAje.Api.Infrastructure.Data
{
    using BackEndAje.Api.Domain.Entities;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<AppUser> AppUsers { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<RolePermission> RolePermissions { get; set; }

        public DbSet<Permission> Permissions { get; set; }

        public DbSet<Region> Regions { get; set; }

        public DbSet<Cedi> Cedis { get; set; }

        public DbSet<Zone> Zones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>().ToTable("appusers");

            modelBuilder.Entity<RolePermission>().ToTable("rolepermissions");

            modelBuilder.Entity<UserRole>().ToTable("userroles");

            modelBuilder.Entity<Role>().ToTable("roles");

            modelBuilder.Entity<User>().ToTable("users");

            modelBuilder.Entity<Permission>().ToTable("permissions");

            modelBuilder.Entity<Region>().ToTable("regions");

            modelBuilder.Entity<Cedi>().ToTable("cedis");

            modelBuilder.Entity<Zone>().ToTable("zones");
        }
    }
}

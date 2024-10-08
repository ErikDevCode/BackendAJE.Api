﻿namespace BackEndAje.Api.Infrastructure.Data
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

        public DbSet<MenuGroup> MenuGroups { get; set; }

        public DbSet<MenuItem> MenuItems { get; set; }

        public DbSet<MenuItemAction> MenuItemActions { get; set; }

        public DbSet<Action> Actions { get; set; }

        public DbSet<RoleMenuAccess> RoleMenuAccess { get; set; }

        public DbSet<ReasonRequest> ReasonRequest { get; set; }

        public DbSet<WithDrawalReason> WithDrawalReason { get; set; }

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

            modelBuilder.Entity<MenuGroup>().ToTable("menugroups");

            modelBuilder.Entity<MenuItem>().ToTable("menuitems");

            modelBuilder.Entity<MenuItemAction>().ToTable("menuitemactions");

            modelBuilder.Entity<Action>().ToTable("actions");

            modelBuilder.Entity<RoleMenuAccess>().ToTable("rolemenuaccess");

            modelBuilder.Entity<ReasonRequest>().ToTable("reasonrequest");

            modelBuilder.Entity<WithDrawalReason>().ToTable("withdrawalreason");

            modelBuilder.Entity<MenuGroup>()
                .HasMany(mg => mg.MenuItems)
                .WithOne(mi => mi.MenuGroup)
                .HasForeignKey(mi => mi.MenuGroupId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MenuItem>()
                .HasOne(mi => mi.ParentItem)
                .WithMany(mi => mi.ChildItems)
                .HasForeignKey(mi => mi.ParentMenuItemId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MenuItem>()
                .HasMany(mi => mi.MenuItemActions)
                .WithOne(mia => mia.MenuItem)
                .HasForeignKey(mia => mia.MenuItemId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Action>()
                .HasMany(a => a.MenuItemActions)
                .WithOne(mia => mia.Action)
                .HasForeignKey(mia => mia.ActionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RoleMenuAccess>()
                .HasOne(rma => rma.RolePermission)
                .WithMany(rp => rp.RoleMenuAccess)
                .HasForeignKey(rma => rma.RolePermissionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MenuItemAction>()
                .HasOne(mp => mp.Action)
                .WithMany(a => a.MenuItemActions)
                .HasForeignKey(mp => mp.ActionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

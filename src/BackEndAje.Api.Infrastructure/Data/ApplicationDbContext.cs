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

        public DbSet<MenuGroup> MenuGroups { get; set; }

        public DbSet<MenuItem> MenuItems { get; set; }

        public DbSet<MenuItemAction> MenuItemActions { get; set; }

        public DbSet<Action> Actions { get; set; }

        public DbSet<RoleMenuAccess> RoleMenuAccess { get; set; }

        public DbSet<ReasonRequest> ReasonRequest { get; set; }

        public DbSet<WithDrawalReason> WithDrawalReason { get; set; }

        public DbSet<TimeWindow> TimeWindows { get; set; }

        public DbSet<ProductType> ProductTypes { get; set; }

        public DbSet<Logo> Logos { get; set; }

        public DbSet<ProductSize> ProductSize { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Province> Provinces { get; set; }

        public DbSet<District> Districts { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<PaymentMethods> PaymentMethods { get; set; }

        public DbSet<DocumentType> DocumentType { get; set; }

        public DbSet<OrderRequest> OrderRequests { get; set; }

        public DbSet<OrderRequestDocument> OrderRequestDocuments { get; set; }

        public DbSet<OrderStatus> OrderStatus { get; set; }

        public DbSet<OrderRequestStatusHistory> OrderRequestStatusHistory { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<Asset> Assets { get; set; }

        public DbSet<ClientAssets> ClientAssets { get; set; }

        public DbSet<ClientAssetsTrace> ClientAssetsTrace { get; set; }

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

            modelBuilder.Entity<TimeWindow>().ToTable("timewindows");

            modelBuilder.Entity<ProductType>().ToTable("producttypes");

            modelBuilder.Entity<Logo>().ToTable("logos");

            modelBuilder.Entity<ProductSize>().ToTable("productsize");

            modelBuilder.Entity<Department>().ToTable("departments");

            modelBuilder.Entity<Province>().ToTable("provinces");

            modelBuilder.Entity<District>().ToTable("districts");

            modelBuilder.Entity<Client>().ToTable("clients");

            modelBuilder.Entity<PaymentMethods>().ToTable("paymentmethods");

            modelBuilder.Entity<DocumentType>().ToTable("documenttype");

            modelBuilder.Entity<OrderRequest>().ToTable("orderrequests");

            modelBuilder.Entity<OrderRequestDocument>().ToTable("orderrequestDocuments");

            modelBuilder.Entity<OrderStatus>().ToTable("orderstatus");

            modelBuilder.Entity<OrderRequestStatusHistory>().ToTable("orderrequeststatushistory");

            modelBuilder.Entity<Position>().ToTable("positions");

            modelBuilder.Entity<Asset>().ToTable("assets");

            modelBuilder.Entity<ClientAssets>().ToTable("clientassets");

            modelBuilder.Entity<ClientAssetsTrace>().ToTable("clientassetstrace");

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

            modelBuilder.Entity<PaymentMethods>()
                .ToTable("paymentmethods")
                .HasKey(pm => pm.PaymentMethodId);

            modelBuilder.Entity<DocumentType>()
                .ToTable("documenttype")
                .HasKey(pm => pm.DocumentTypeId);

            modelBuilder.Entity<Client>()
                .HasOne(c => c.DocumentType)
                .WithMany()
                .HasForeignKey(c => c.DocumentTypeId)
                .IsRequired(false);

            modelBuilder.Entity<Client>()
                .HasOne(c => c.PaymentMethod)
                .WithMany()
                .HasForeignKey(c => c.PaymentMethodId)
                .IsRequired(false);

            modelBuilder.Entity<Client>()
                .HasOne(c => c.District)
                .WithMany()
                .HasForeignKey(c => c.DistrictId)
                .IsRequired(false);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Cedi)
                .WithMany()
                .HasForeignKey(u => u.CediId)
                .IsRequired(false);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Zone)
                .WithMany()
                .HasForeignKey(u => u.ZoneId)
                .IsRequired(false);

            modelBuilder.Entity<Cedi>()
                .HasOne(u => u.Region)
                .WithMany()
                .HasForeignKey(u => u.RegionId);

            modelBuilder.Entity<OrderRequest>()
                .ToTable("orderrequests")
                .HasKey(pm => pm.OrderRequestId);

            modelBuilder.Entity<OrderRequestDocument>()
                .ToTable("orderrequestdocuments")
                .HasKey(pm => pm.DocumentId);

            modelBuilder.Entity<OrderRequestStatusHistory>()
                .ToTable("orderrequeststatushistory")
                .HasKey(pm => pm.OrderStatusHistoryId);

            modelBuilder.Entity<OrderRequestStatusHistory>()
                .HasOne(history => history.CreatedByUser)
                .WithMany()
                .HasForeignKey(history => history.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClientAssets>()
                .ToTable("clientassets")
                .HasKey(pm => pm.ClientAssetId);

            modelBuilder.Entity<ClientAssetsTrace>()
                .ToTable("clientassetstrace")
                .HasKey(pm => pm.ClientAssetTraceId);
        }
    }
}

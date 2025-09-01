using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.Models;

namespace Ultimate_POS_Api.Data
{
    public class UltimateDBContext : DbContext
    {
        public UltimateDBContext(DbContextOptions<UltimateDBContext> options) : base(options) { }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Payments> payments { get; set; }
        public DbSet<PaymentDetails> PaymentDetails { get; set; }
        public DbSet<Catalogue> Catalogue { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
        public DbSet<TransactionProducts> TransactionProducts { get; set; }
        public DbSet<Supplier> Supplier { get; set; }
        public DbSet<Supplies> Supplies { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<Contacts> Contacts { get; set; }
        public DbSet<MessageTemplates> MessageTemplates { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Permissions> Permission { get; set; }
        public DbSet<RolePermissions> RolePermission { get; set; }
        public DbSet<UserRoles> UserRole { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }
        public DbSet<Till> tills { get; set; }
        public DbSet<TillPaymentSummary> tillPaymentSummary { get; set; }
        public DbSet<UploadedFiles> Documents { get; set; }
        public DbSet<BusinessDetail> BusinessDetail { get; set; }
        public DbSet<Accounts> Accounts { get; set; }

        public DbSet<AccountTrxSettlement> AccountTrxSettlement { get; set; }


        public DbSet<Logs> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.Entity<Products>()
            //     .HasOne(p => p.Supplier)
            //     .WithMany()
            //     .HasForeignKey(p => p.SupplierId);

            modelBuilder.Entity<Products>()
            .HasOne(p => p.Supplier)
            .WithMany(s => s.Products)
             .HasForeignKey(p => p.SupplierId);


            modelBuilder.Entity<Products>()
                .HasOne(p => p.Categories)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryID);

            modelBuilder.Entity<Catalogue>()
                .HasKey(ur => ur.SKU);

            //Prevent duplicate role names

            modelBuilder.Entity<Roles>()
                .HasIndex(ur => ur.RoleName)
                .IsUnique();

            modelBuilder.Entity<RolePermissions>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.Permissions)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<RolePermissions>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);

            //prevent duplicate role-permission combinations
            modelBuilder.Entity<RolePermissions>()
                .HasIndex(rp => new { rp.RoleId, rp.PermissionId })
                .IsUnique();

            //modelBuilder.Entity<UserRoles>()
            //  .HasOne(ur => ur.User)
            //  .WithOne(u => u.UserRoles)
            //  .HasForeignKey(ur => ur.UserId)
            //  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserRoles>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserRoles>()
              .HasIndex(ur => new { ur.UserId, ur.RoleId })
              .IsUnique();

            modelBuilder.Entity<Till>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.Tills)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);




            //modelBuilder.Entity<User>()
            //    .HasOne(ur => ur.Role);

            modelBuilder.Entity<Supplier>()
                .HasKey(ur => ur.SupplierId);


            // modelBuilder.Entity<Catalogue>()
            // .HasOne(p => p.Products)
            // .WithMany(c => c.Catalogues)
            // .HasForeignKey(p => p.SKU);

            modelBuilder.Entity<Catalogue>()
            .HasOne(c => c.Products)
            .WithMany(p => p.Catalogues)
            .HasForeignKey(c => c.ProductId);


            base.OnModelCreating(modelBuilder);
        }
    }

    // // Design-time factory for EF Core CLI
    // public class UltimateDbContextFactory : IDesignTimeDbContextFactory<UltimateDBContext>
    // {
    //     public UltimateDBContext CreateDbContext(string[] args)
    //     {
    //         var optionsBuilder = new DbContextOptionsBuilder<UltimateDBContext>();

    //         optionsBuilder.UseNpgsql(
    //             "Host=ep-rapid-shape-a4hjzcue-pooler.us-east-1.aws.neon.tech;" +
    //             "Database=ultimate;" +
    //             "Username=ultimate_owner;" +
    //             "Password=npg_EMx5VpIThtl3;" +
    //             "Ssl Mode=Require"
    //         );

    //         return new UltimateDBContext(optionsBuilder.Options);
    //     }
    // }
}

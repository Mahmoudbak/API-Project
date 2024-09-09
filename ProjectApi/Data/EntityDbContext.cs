using Microsoft.EntityFrameworkCore;
using ProjectApi.Model;

namespace ProjectApi.Data
{
    public class EntityDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        public EntityDbContext(DbContextOptions<EntityDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure many-to-many relationship between Order and Product
            modelBuilder.Entity<OrderDetail>()
                .HasKey(od => new { od.OrderId, od.ProductId });

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId);

            //modelBuilder.Entity<OrderDetail>()
            //    .HasOne(od => od.Product)
            //    .WithMany(o => o.OrderDetails)
            //    .HasForeignKey(od => od.ProductId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customers)
                .WithMany(c => c.orders)
                .HasForeignKey(o => o.CustomerId);

            modelBuilder.Entity<Product>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Order>()
                .Property(o => o.OrderDate)
                .HasDefaultValueSql("GETDATE()"); // For SQL Server default date

            modelBuilder.Entity<Order>()
                .Property(o => o.Status)
                .HasConversion<int>(); // Store enum as integer in the database

            base.OnModelCreating(modelBuilder);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using OrderManagement.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Reflection;
using OrderManagement.Core.Entities.Identity;

namespace OrderManagement.Repository.Data
{
    public class OrderManagementDbContext:IdentityDbContext
    {
        public OrderManagementDbContext(DbContextOptions<OrderManagementDbContext>Optoins):base(Optoins) 
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().Property(p => p.Price).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Inovice>().Property(p => p.TotalAmount).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<OrderItem>().Property(p => p.UnitPrice).HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Order>().Property(o => o.Status)
                .HasConversion(OS => OS.ToString(), OS => (OrderStatus)Enum.Parse(typeof(OrderStatus), OS));

            modelBuilder.Entity<Order>().Property(p => p.TotalAmount).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<OrderItem>().Property(p => p.Discount).HasColumnType("decimal(18,2)");
            //modelBuilder.Entity<Order>().HasOne(O => O.Customer).WithMany()
            //    .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Order>().HasMany(o => o.OrderItems)
                .WithOne().HasForeignKey(o => o.OrderId).IsRequired(false)
                .OnDelete(deleteBehavior: DeleteBehavior.SetNull);
            

            //modelBuilder.Entity<Order>().HasOne(o => o.Customer)
            //    .WithMany().HasForeignKey(o => o.CustomerId).IsRequired(false)
            //    .OnDelete(deleteBehavior: DeleteBehavior.SetNull);

            //modelBuilder.Entity<Customer>().HasMany(o => o.Orders)
            //    .WithOne().HasForeignKey(o => o.CustomerId).IsRequired(false)
            //    .OnDelete(deleteBehavior: DeleteBehavior.SetNull);

            

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Inovice> Inovices { get; set; }
    }
}

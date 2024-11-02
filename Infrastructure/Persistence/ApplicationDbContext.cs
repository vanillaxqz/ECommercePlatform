using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> Images { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>().HasKey(a => a.AdminId);
            modelBuilder.Entity<Admin>()
                .Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<Admin>()
                .Property(a => a.Email)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<Admin>()
                .Property(a => a.Password)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<User>().HasKey(u => u.UserId);
            modelBuilder.Entity<User>()
                .Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<User>()
                .Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<User>()
                .Property(u => u.Address)
                .HasMaxLength(200);
            modelBuilder.Entity<User>()
                .Property(u => u.PhoneNumber)
                .HasMaxLength(15);

            modelBuilder.Entity<Product>().HasKey(p => p.ProductId);
            modelBuilder.Entity<Product>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<Product>()
                .Property(p => p.Description)
                .HasMaxLength(500);
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .IsRequired();
            modelBuilder.Entity<Product>()
                .Property(p => p.Stock)
                .IsRequired();

            modelBuilder.Entity<ProductImage>().HasKey(pi => pi.ProductImageId);
            modelBuilder.Entity<ProductImage>()
                .Property(pi => pi.ImagePath)
                .IsRequired()
                .HasMaxLength(200);
            modelBuilder.Entity<ProductImage>()
                .HasOne<Product>()
                .WithMany()
                .HasForeignKey(pi => pi.ProductId);

            modelBuilder.Entity<Order>().HasKey(o => o.OrderId);
            modelBuilder.Entity<Order>()
                .Property(o => o.OrderDate)
                .IsRequired();
            modelBuilder.Entity<Order>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(o => o.UserId);
            modelBuilder.Entity<Order>()
                .HasOne<Payment>()
                .WithOne()
                .HasForeignKey<Order>(o => o.PaymentId);

            modelBuilder.Entity<OrderItem>().HasKey(oi => oi.OrderItemId);
            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.Name)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.Description)
                .HasMaxLength(500);
            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.Price)
                .IsRequired();
            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.ImagePath)
                .HasMaxLength(200);
            modelBuilder.Entity<OrderItem>()
                .HasOne<Order>()
                .WithMany()
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<Payment>().HasKey(p => p.PaymentId);
            modelBuilder.Entity<Payment>()
                .Property(p => p.PaymentDate)
                .IsRequired();
            modelBuilder.Entity<Payment>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<Session>().HasKey(s => s.SessionId);
            modelBuilder.Entity<Session>()
                .Property(s => s.StartTime)
                .IsRequired();
            modelBuilder.Entity<Session>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(s => s.UserId);
        }
    }
}

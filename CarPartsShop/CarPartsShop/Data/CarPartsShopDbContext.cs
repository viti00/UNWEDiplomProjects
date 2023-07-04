using CarPartsShop.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace CarPartsShop.Data
{
    public class CarPartsShopDbContext : IdentityDbContext<ApplicationUser>
    {
        public CarPartsShopDbContext(DbContextOptions<CarPartsShopDbContext> options)
            : base(options)
        {
        }

        public DbSet<PartType> PartTypes { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<PartCondtion> PartCondtions { get; set; }
        public DbSet<PartImage> PartImages { get; set; }
        public DbSet<CartStatus> CartStatuses { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<log_19118067> log_19118067 { get; set; }

        public DbSet<PickedUpPart> PickedUpParts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Part>().ToTable("Parts", "19118067");
            builder.Entity<PartCondtion>().ToTable("PartConditions", "19118067");
            builder.Entity<PartType>().ToTable("PartTypes", "19118067");
            builder.Entity<Order>().ToTable("Orders","19118067");
            builder.Entity<Cart>().ToTable("Carts", "19118067");
            builder.Entity<OrderStatus>().ToTable("OrderStatuses", "19118067");
            builder.Entity<PartImage>().ToTable("PartImages", "19118067");
            builder.Entity<CartStatus>().ToTable("CartStatuses", "19118067");
            builder.Entity<PickedUpPart>().ToTable("PickedUpParts", "19118067");
            builder.Entity<log_19118067>().ToTable("log_19118067", "19118067");

            builder.Entity<PickedUpPart>()
                .HasKey(x => new { x.PartId, x.CartId });

            base.OnModelCreating(builder);
        }

    }
}
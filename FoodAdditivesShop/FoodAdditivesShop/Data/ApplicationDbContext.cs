using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FoodAdditivesShop.Data;

namespace FoodAdditivesShop.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartProduct> CartProducts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<log_19118119> log_19118119 { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<Product>().ToTable("Products", "19118119");

            builder.Entity<Cart>().ToTable("Carts", "19118119");

            builder.Entity<CartProduct>().ToTable("CartProducts", "19118119");

            builder.Entity<Order>().ToTable("Orders", "19118119");


            builder.Entity<log_19118119>().ToTable("log_19118119", "19118119");

            builder.Entity<CartProduct>()
                .HasKey(x => new { x.ProductId, x.CartId });

            base.OnModelCreating(builder);
        }
    }
}
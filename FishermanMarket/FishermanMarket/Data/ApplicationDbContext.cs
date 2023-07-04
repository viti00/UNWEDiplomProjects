using FishermanMarket.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;

namespace FishermanMarket.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Bag> Bags { get; set; }
        public DbSet<BagProduct> BagProducts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<log_19118062> log_19118062 { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Category>().ToTable("Categories", "19118062");
            builder.Entity<Product>().ToTable("Products", "19118062");
            builder.Entity<ProductImage>().ToTable("ProductImages", "19118062");
            builder.Entity<Bag>().ToTable("Bag", "19118062");
            builder.Entity<BagProduct>().ToTable("BagProducts", "19118062");
            builder.Entity<Order>().ToTable("Orders", "19118062");
            builder.Entity<log_19118062>().ToTable("log_19118062", "19118062");
            builder.Entity<Request>().ToTable("Requests", "19118062");

            builder.Entity<BagProduct>()
                 .HasKey(x => new { x.ProductId, x.BagId });

            base.OnModelCreating(builder);
        }
    }
}
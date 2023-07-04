using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VBoutique.Data;

namespace VBoutique.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Shoe> Shoes { get; set; }
        public DbSet<Bag> Bags { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<ShoeSize> ShoeSizes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShoppingCartItems> ShoppingCartItems { get; set; }
        public DbSet<log_19118155> log_19118155 { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Shoe>().ToTable("Shoes", "19118155");
            builder.Entity<Bag>().ToTable("Bags", "19118155");
            builder.Entity<Color>().ToTable("Colors", "19118155");
            builder.Entity<Size>().ToTable("Sizes", "19118155");
            builder.Entity<ShoeSize>().ToTable("ShoeSizes", "19118155");
            builder.Entity<Order>().ToTable("Orders", "19118155");
            builder.Entity<ShoppingCart>().ToTable("ShoppingCarts", "19118155");
            builder.Entity<ShoppingCartItems>().ToTable("ShoppingCartItems", "19118155");
            builder.Entity <log_19118155>().ToTable("log_19118155", "19118155");
            


            builder.Entity<ShoppingCartItems>()
                 .HasKey(x => new { x.ProductId, x.CartId });

            builder.Entity<ShoeSize>()
                 .HasKey(x => new { x.ShoeId, x.SizeId });

            base.OnModelCreating(builder);
        }

        public DbSet<VBoutique.Data.Product>? Product { get; set; }
    }
}
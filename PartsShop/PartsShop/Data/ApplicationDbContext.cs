using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;

namespace PartsShop.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartPart> CartParts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<PartReview> PartReviews { get; set; }
        public DbSet<log_19118073> log_19118073 { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            
            builder.Entity<Category>().ToTable("Categories", "19118073");

            builder.Entity<Part>().ToTable("Parts", "19118073");

            builder.Entity<Cart>().ToTable("Carts", "19118073");

            builder.Entity<CartPart>().ToTable("CartParts", "19118073");

            builder.Entity<Order>().ToTable("Orders", "19118073");

            builder.Entity<PartReview>().ToTable("PartReviews", "19118073");

            builder.Entity<log_19118073>().ToTable("log_19118073", "19118073");

            builder.Entity<CartPart>()
                .HasKey(x => new { x.PartId, x.CartId });

            base.OnModelCreating(builder);
        }
    }
}
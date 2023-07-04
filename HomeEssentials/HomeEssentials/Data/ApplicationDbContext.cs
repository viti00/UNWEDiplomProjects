using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HomeEssentials.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<log_19118075> log_19118075 { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<ItemInCart> ItemsInCart { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<CartStatus> CartStatuses { get; set; }
        public DbSet<OrderStatuses> OrderStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ItemCategory>().ToTable("ItemCategories", "19118075");
            builder.Entity<log_19118075>().ToTable("log_19118075", "19118075");
            builder.Entity<Item>().ToTable("Items", "19118075");
            builder.Entity<Cart>().ToTable("Carts", "19118075");
            builder.Entity<ItemInCart>().ToTable("ItemsInCart", "19118075");
            builder.Entity<Order>().ToTable("Orders", "19118075");
            builder.Entity<Review>().ToTable("Reviews", "19118075");
            builder.Entity<CartStatus>().ToTable("CartStatuses", "19118075");
            builder.Entity<OrderStatuses>().ToTable("OrderStatuses", "19118075");


            builder.Entity<ItemInCart>()
                 .HasKey(x => new { x.ItemId, x.CartId });

            base.OnModelCreating(builder);
        }
    }
}
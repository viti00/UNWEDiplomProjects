using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RestaurantOrders.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Bag> Bags { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<SelectedProduct> SelectedProducts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<log_19118066> log_19118066 { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Category>().ToTable("Categories", "19118066");
            builder.Entity<Bag>().ToTable("Bags", "19118066");
            builder.Entity<Meal>().ToTable("Meals", "19118066");
            builder.Entity<SelectedProduct>().ToTable("SelectedProducts", "19118066");
            builder.Entity<Request>().ToTable("Requests", "19118066");
            builder.Entity<Order>().ToTable("Orders", "19118066");
            builder.Entity<log_19118066>().ToTable("log_19118066", "19118066");


            builder.Entity<SelectedProduct>()
                 .HasKey(x => new { x.MealId, x.BagId });

            base.OnModelCreating(builder);
        }
    }
}
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SteliTour.Data;

namespace SteliTour.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Destination> Destination { get; set; }
        public DbSet<Request> Request { get; set; }
        public DbSet<Reservation> Reservation { get; set; }
        public DbSet<Review> Review { get; set; }
        public DbSet<log_19118105> log_19118105 { get; set; }
        public DbSet<DenidedReservation> DenidedReservations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Destination>().ToTable("Destinations", "19118105");
            builder.Entity<Request>().ToTable("Requests", "19118105");
            builder.Entity<Reservation>().ToTable("Reservations", "19118105");
            builder.Entity<Review>().ToTable("Reviews", "19118105");
            builder.Entity<log_19118105>().ToTable("log_19118105", "19118105");
            builder.Entity<DenidedReservation>().ToTable("DenidedReservations", "19118105");

            builder.Entity<Reservation>()
                 .HasKey(x => new { x.DestinationId, x.UserId });

            base.OnModelCreating(builder);
        }
    }
}
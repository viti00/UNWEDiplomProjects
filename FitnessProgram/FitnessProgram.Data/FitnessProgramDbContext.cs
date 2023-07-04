namespace FitnessProgram.Data
{
    using FitnessProgram.Data.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;


    public class FitnessProgramDbContext : IdentityDbContext<User>
    {
        public DbSet<Post> Posts { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Partner> Partners { get; set; }

        public DbSet<BestResult> BestResults { get; set; }

        public DbSet<UserLikedPost> UserLikedPosts { get; set; }

        public DbSet<ProfilePhoto> ProfilePhotos { get; set; }

        public DbSet<PostPhoto> PostPhotos { get; set; }

        public DbSet<BestResultPhoto> BestResultPhotos { get; set; }

        public DbSet<PartnerPhoto> PartnerPhotos { get; set; }
        public DbSet<log_19118074> log_19118074 { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<TimeRange> TimeRanges { get; set; }
        public DbSet<DateOfReservation> DateOfReservations { get; set; }
        public DbSet<ReservationDateTimeRange> ReservationDateTimeRange { get; set; }


        public FitnessProgramDbContext(DbContextOptions<FitnessProgramDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Post>().ToTable("Posts", "19118074");
            builder.Entity<Comment>().ToTable("Comments", "19118074");
            builder.Entity<Partner>().ToTable("Partners", "19118074");
            builder.Entity<BestResult>().ToTable("BestResults", "19118074");
            builder.Entity<UserLikedPost>().ToTable("UserLikedPosts", "19118074");
            builder.Entity<ProfilePhoto>().ToTable("ProfilePhotos", "19118074");
            builder.Entity<PostPhoto>().ToTable("PostPhoto", "19118074");
            builder.Entity<BestResultPhoto>().ToTable("BestResultPhotos", "19118074");
            builder.Entity<PartnerPhoto>().ToTable("PartnerPhotos", "19118074");
            builder.Entity<log_19118074>().ToTable("log_19118074", "19118074");
            builder.Entity<Reservation>().ToTable("Reservations", "19118074");
            builder.Entity<TimeRange>().ToTable("TimeRanges", "19118074");
            builder.Entity<DateOfReservation>().ToTable("DateOfReservations", "19118074");
            builder.Entity<ReservationDateTimeRange>().ToTable("ReservationDateTimeRange", "19118074");

            builder.Entity<UserLikedPost>()
                .HasKey(k => new { k.UserId, k.PostId });
            builder.Entity<ReservationDateTimeRange>()
                .HasKey(k => new { k.DateOfReservationId, k.TimeRangeId });

            builder.Entity<UserLikedPost>()
                .HasOne(x => x.Post)
                .WithMany(x => x.Likes)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Comment>()
                .HasOne(x => x.Post)
                .WithMany(x => x.Comments)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
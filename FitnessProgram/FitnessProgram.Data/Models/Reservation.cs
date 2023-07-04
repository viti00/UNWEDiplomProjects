


namespace FitnessProgram.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("Reservations", Schema = "19118074")]
    public class Reservation
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string DateId { get; set; }

        public int RangeId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Address { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public virtual User User { get; set; }

        public DateTime LastAvailableForModification { get; set; }

        [Required]
        public DateTime LastModified_19118074 { get; set; }
    }
}


namespace FitnessProgram.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("DateOfReservations", Schema = "19118074")]
    public class DateOfReservation
    {
        [Required]
        public string Id { get; set; }

        public List<ReservationDateTimeRange> TimeRanges { get; set; } = new List<ReservationDateTimeRange>();

        [Required]
        public DateTime LastModified_19118074 { get; set; }
    }
}

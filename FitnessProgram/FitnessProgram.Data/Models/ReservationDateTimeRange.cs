using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessProgram.Data.Models
{
    [Table("ReservationDateTimeRange", Schema = "19118074")]
    public class ReservationDateTimeRange
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [ForeignKey(nameof(DateOfReservation))]
        public string DateOfReservationId { get; set; }

        [Required]
        [ForeignKey(nameof(TimeRange))]
        public int TimeRangeId { get; set; }

        public DateOfReservation DateOfReservation { get; set; }

        public TimeRange TimeRange { get; set; }

        public string Status { get; set; }

        [Required]
        public DateTime LastModified_19118074 { get; set; }
    }
}





namespace FitnessProgram.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TimeRanges", Schema = "19118074")]
    public class TimeRange
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RangeId { get; set; }

        [Required]
        public string RangeName { get; set; }

        [Required]
        public DateTime LastModified_19118074 { get; set; }

    }
}

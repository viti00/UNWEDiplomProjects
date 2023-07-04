namespace FitnessProgram.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using static FitnessProgram.Global.GlobalConstants;

    [Table("BestResults", Schema = "19118074")]
    public class BestResult
    {
        public int Id { get; set; }

        [Required]
        public List<BestResultPhoto> Photos { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        [MaxLength(BestResultConstants.StoryMaxLegth)]
        public string Story { get; set; }

        [Required]
        public DateTime LastModified_19118074 { get; set; }

        public int RowVersion { get; set; } = 1;
    }
}

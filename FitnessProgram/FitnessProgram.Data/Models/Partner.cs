namespace FitnessProgram.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using static FitnessProgram.Global.GlobalConstants;

    [Table("Partners", Schema = "19118074")]
    public class Partner
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(PartnerConstants.NameMaxLegth)]
        public string Name { get; set; }

        [Required]
        [MaxLength(PartnerConstants.DescriptionMaxLegth)]
        public string Description { get; set; }

        [Required]
        public PartnerPhoto Photo { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        [MaxLength(PartnerConstants.PromoCodeMaxLegth)]
        public string PromoCode { get; set; }

        public DateTime CreatedOn { get; set; }

        public int RowVersion { get; set; } = 1;

        [Required]
        public DateTime LastModified_19118074 { get; set; }
    }
}

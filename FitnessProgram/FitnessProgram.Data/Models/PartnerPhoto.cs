namespace FitnessProgram.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ParnerPhotos", Schema = "19118074")]
    public class PartnerPhoto : Photo
    {
        [ForeignKey(nameof(Partner))]
        public int PartnerId { get; set; }

        public virtual Partner Partner { get; set; }
    }
}

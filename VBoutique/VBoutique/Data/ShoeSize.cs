using System.ComponentModel.DataAnnotations.Schema;

namespace VBoutique.Data
{
    [Table("ShoeSizes", Schema = "19118155")]
    public class ShoeSize
    {
        [ForeignKey("Shoe")]
        public Guid ShoeId { get; set; }

        public Shoe Shoe { get; set; }

        [ForeignKey("Size")]
        public int SizeId { get; set; }

        public Size Size { get; set; }

        public int AvailableCount { get; set; }

        public DateTime? LastModified_19118155 { get; set; }
    }
}

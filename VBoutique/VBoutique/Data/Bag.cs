using System.ComponentModel.DataAnnotations.Schema;

namespace VBoutique.Data
{
    [Table("Bags", Schema = "19118155")]
    public class Bag : Product
    {
        public int AvailableCount {get;set; }

        [ForeignKey("Color")]
        public int ColorId { get; set; }
        public Color? Color { get; set; }
    }
}

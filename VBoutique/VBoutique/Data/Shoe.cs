using System.ComponentModel.DataAnnotations.Schema;

namespace VBoutique.Data
{
    [Table("Shoes", Schema = "19118155")]
    public class Shoe : Product
    {
        public List<ShoeSize>? ShoeSizes { get; set; }
        [ForeignKey("Color")]
        public int ColorId { get; set; }
        public Color? Color { get; set; }
    }
}

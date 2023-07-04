using System.ComponentModel.DataAnnotations.Schema;

namespace VBoutique.Data
{
    [Table("Sizes", Schema = "19118155")]
    public class Size
    {
        public int Id { get; set; }

        public string Value { get; set; }

        public List<ShoeSize> ShoeSizes { get; set; }

        public DateTime? LastModified_19118155 { get; set; }
    }
}

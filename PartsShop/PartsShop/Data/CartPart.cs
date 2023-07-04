using System.ComponentModel.DataAnnotations.Schema;

namespace PartsShop.Data
{
    [Table("CartParts", Schema = "19118073")]
    public class CartPart
    {
        public Guid Id { get; set; }

        [ForeignKey("Part")]
        public Guid PartId { get; set; }

        public Part Part { get; set; }

        [ForeignKey("Cart")]
        public Guid CartId { get; set; }

        public Cart Cart { get; set; }

        public int PartCount { get; set; }

        public DateTime LastModified_19118073 { get; set; }
    }
}

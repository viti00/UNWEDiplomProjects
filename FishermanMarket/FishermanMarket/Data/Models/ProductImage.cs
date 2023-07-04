using System.ComponentModel.DataAnnotations.Schema;

namespace FishermanMarket.Data.Models
{
    [Table("ProductImages", Schema = "19118062")]
    public class ProductImage
    {
        public Guid Id { get; set; }

        public byte[] Bytes { get; set; }

        public string Description { get; set; }

        public string FileExtension { get; set; }

        public double Size { get; set; }

        [ForeignKey(nameof(Product))]
        public Guid ProductId { get; set; }

        public Product Product { get; set; }

        public DateTime LastModified_19118062 { get; set; }
    }
}

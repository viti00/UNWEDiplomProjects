using System.ComponentModel.DataAnnotations.Schema;

namespace FishermanMarket.Data.Models
{
    [Table("BagProducts", Schema = "19118062")]
    public class BagProduct
    {
        public Guid Id { get; set; }

        [ForeignKey(nameof(Product))]
        public Guid ProductId { get; set; }

        public Product Product { get; set; }

        [ForeignKey(nameof(Bag))]
        public Guid BagId { get; set; }

        public Bag Bag { get; set; }

        public int Quantity { get; set; }

        public DateTime LastModified_19118062 { get; set; }
    }
}

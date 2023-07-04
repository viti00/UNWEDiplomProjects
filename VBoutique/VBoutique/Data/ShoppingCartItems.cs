using System.ComponentModel.DataAnnotations.Schema;

namespace VBoutique.Data
{
    [Table("ShoppingCartItems", Schema = "19118155")]
    public class ShoppingCartItems
    {
        public Guid Id { get; set; }

        [ForeignKey("Cart")]
        public Guid CartId { get; set; }

        public ShoppingCart Cart { get; set; }

        [ForeignKey("Product")]
        public Guid ProductId { get; set; }

        public Product Product { get; set; }

        public int ProductCount { get; set; }

        public int Size35Count { get; set; } = 0;
        public int Size36Count { get; set; } = 0;
        public int Size37Count { get; set; } = 0;
        public int Size38Count { get; set; } = 0;
        public int Size39Count { get; set; } = 0;
        public int Size40Count { get; set; } = 0;
        public int Size41Count { get; set; } = 0;

        public DateTime? LastModified_19118155 { get; set; }
    }
}

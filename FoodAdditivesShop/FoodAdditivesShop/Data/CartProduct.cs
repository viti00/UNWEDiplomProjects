using System.ComponentModel.DataAnnotations.Schema;

namespace FoodAdditivesShop.Data
{
    [Table("CartProducts", Schema = "19118119")]
    public class CartProduct
    {
        public Guid Id { get; set; }

        [ForeignKey("Products")]
        public Guid ProductId { get; set; }

        public Product Product { get; set; }

        [ForeignKey("Cart")]
        public Guid CartId { get; set; }

        public Cart Cart { get; set; }

        public int ProductCount { get; set; }

        public DateTime LastModified_19118119 { get; set; }
    }
}

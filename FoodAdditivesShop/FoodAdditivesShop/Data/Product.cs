using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodAdditivesShop.Data
{
    [Table("Products", Schema = "19118119")]
    public class Product
    {
        public Guid Id { get; set; }

        public string ProductName { get; set; }
        public string ProductMake { get; set; }

        public string ProductDescription { get; set; }

        public string ProductImageUrl { get; set; }

        public double ProductPrice { get; set; }

        public List<CartProduct>? CartProducts { get; set; }

        public DateTime? DateOfAdd { get; set; }

        public DateTime? LastModified_19118119 { get; set; }

    }
}

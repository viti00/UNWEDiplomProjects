using System.ComponentModel.DataAnnotations.Schema;

namespace VBoutique.Data
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string ProductDescription { get; set; }
        public double Price { get; set; }

        public List<ShoppingCartItems>? ShoppingCartItems { get; set; }
        public DateTime? LastModified_19118155 { get; set; }

        public string ImageUrl { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodAdditivesShop.Data
{
    [Table("Carts", Schema = "19118119")]
    public class Cart
    {
        public Guid Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public IdentityUser User { get; set; }

        public List<CartProduct>? CartProducts { get; set; }

        public bool IsOrdered { get; set; } = false;

        public DateTime LastModified_19118119 { get; set; }
    }
}

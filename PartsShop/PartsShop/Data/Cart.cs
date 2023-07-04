using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace PartsShop.Data
{
    [Table("Carts", Schema = "19118073")]
    public class Cart
    {
        public Guid Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public IdentityUser User { get; set; }

        public List<CartPart>? CartParts { get; set; }

        public bool IsOrdered { get; set; } = false;

        public DateTime LastModified_19118073 { get; set; }
    }
}

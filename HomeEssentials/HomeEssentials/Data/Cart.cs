using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeEssentials.Data
{
    [Table("Carts", Schema = "19118075")]
    public class Cart
    {
        public Guid Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public IdentityUser? User { get; set; }

        [ForeignKey("Status")]
        public int CartStatusId { get; set; }

        public CartStatus? Status { get; set; }

        public List<ItemInCart>? CartItems { get; set; }

        public DateTime? LastModified_19118075 { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace VBoutique.Data
{
    [Table("ShoppingCart", Schema = "19118155")]
    public class ShoppingCart
    {
        public Guid Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public IdentityUser User { get; set; }

        public List<ShoppingCartItems>? ShoppingCartItems { get; set; }

        public bool IsActive { get; set; }
        public DateTime? LastModified_19118155 { get; set; }
    }
}

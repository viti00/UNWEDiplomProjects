using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarPartsShop.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Order> Orders { get; set; } = new HashSet<Order>();

        public ICollection<Cart> Carts { get; set; } = new HashSet<Cart>();
    }
}

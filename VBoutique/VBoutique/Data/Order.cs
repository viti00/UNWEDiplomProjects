using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace VBoutique.Data
{
    [Table("Orders", Schema = "19118155")]
    public class Order
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DeliveryAddress { get; set; }

        public string PhoneNumber { get; set; }

        public Guid ShoppingCartId { get; set;}

        [ForeignKey("User")]
        public string UserId { get; set; }

        public IdentityUser? User { get; set; }

        public DateTime? LastModified_19118155 { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace FishermanMarket.Data.Models
{
    [Table("Orders", Schema = "19118062")]
    public class Order
    {
        public Guid Id { get; set; }

        public string? Status { get; set; }

        [ForeignKey(nameof(User))]
        public string? UserId { get;set; }

        public IdentityUser? User { get; set; }
        public Guid BagId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        public string DeliveryAddres { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime LastModified_19118062 { get; set; }
    }
}



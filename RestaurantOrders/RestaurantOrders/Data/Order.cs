using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantOrders.Data
{
    [Table("Orders", Schema = "19118066")]
    public class Order
    {
        public Guid Id { get; set; }

        public string? Status { get; set; }

        [ForeignKey(nameof(User))]
        public string? UserId { get; set; }

        public IdentityUser? User { get; set; }
        public Guid BagId { get; set; }

        public string PhoneNumber { get; set; }
        public string DeliveryAddres { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? DesiredDeliveryTime { get; set; }

        public DateTime OrderDate { get; set; }
        public DateTime LastModified_19118066 { get; set; }
    }
}

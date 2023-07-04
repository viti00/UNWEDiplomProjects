using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarPartsShop.Data.Models
{
    [Table("Orders", Schema = "19118067")]
    public class Order
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime LastModified_19118067 { get; set; }

        [Required(ErrorMessage ="Полето е задължително")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        public string PostCode { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        public string City { get; set; }

        public bool IsPaid { get; set; }

        public List<PickedUpPart> Parts { get; set; }

        [ForeignKey(nameof(OrderStatus))]
        public int OrderStatusId { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
}

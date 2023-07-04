using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantOrders.Data
{
    [Table("Requests", Schema = "19118066")]
    public class Request
    {
        public Guid Id { get; set; }

        public string FullName { get; set; }
        public string PhoneNumber { get; set; }

        public string RequestText { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public IdentityUser User { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfEvent { get; set; }

        public DateTime? RequestDate { get; set; }

        public DateTime? LastModified_19118066 { get; set; }
    }
}

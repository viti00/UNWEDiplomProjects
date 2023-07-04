using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeEssentials.Data
{
    [Table("Orders", Schema = "19118075")]
    public class Order
    {
        public Guid Id { get; set; }

        public Guid? CartId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public IdentityUser? User { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public OrderStatuses? Status { get; set; }
        public DateTime? CreatedOn { get; set; }

        public DateTime? LastModified_19118075 { get; set; }
    }
}

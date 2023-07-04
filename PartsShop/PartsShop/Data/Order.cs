using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace PartsShop.Data
{
    [Table("Orders", Schema = "19118073")]
    public class Order
    {
        public Guid Id { get; set; }

        public string FName { get; set; }

        public string LName { get; set; }

        public string Phone { get; set; }

        public string Addres { get; set; }

        public Guid? CartId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public IdentityUser? User { get; set; }
        public DateTime? LastModified_19118073 { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantOrders.Data
{
    [Table("Bags", Schema = "19118066")]
    public class Bag
    {
        public Guid Id { get; set; }
        public string Status { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        public List<SelectedProduct> Meals { get; set; }
        public DateTime LastModified_19118066 { get; set; }
    }
}

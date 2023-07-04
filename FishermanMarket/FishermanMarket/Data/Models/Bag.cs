using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace FishermanMarket.Data.Models
{

    [Table("Bags", Schema = "19118062")]
    public class Bag
    {
        public Guid Id { get; set; }

        //статусите са O/C
        public string Status { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        public List<BagProduct> Products { get; set; }
        public DateTime LastModified_19118062 { get; set; }

    }
}

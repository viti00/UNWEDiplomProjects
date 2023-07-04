using System.ComponentModel.DataAnnotations.Schema;

namespace CarPartsShop.Data.Models
{
    [Table("Carts", Schema = "19118067")]

    public class Cart
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [ForeignKey(nameof(CartStatus))]
        public int CartStatusId { get; set; }

        public CartStatus CartStatus { get; set;}

        public ICollection<PickedUpPart> Parts { get; set; } = new HashSet<PickedUpPart>();

        public DateTime LastModified_19118067 { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}

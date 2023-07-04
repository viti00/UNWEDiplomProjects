using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace PartsShop.Data
{
    [Table("PartReviews", Schema = "19118073")]
    public class PartReview
    {
        public Guid Id { get; set; }

        public string ReviewText { get; set; }

        public Guid PartId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public IdentityUser? User { get; set; }

        public DateTime? LastModified_19118073 { get; set; }
    }
}

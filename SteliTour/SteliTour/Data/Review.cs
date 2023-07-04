using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SteliTour.Data
{
    [Table("Reviews", Schema = "19118105")]
    public class Review
    {
        public Guid Id { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public IdentityUser? User { get; set; }

        public string ReviewText { get; set; }

        public DateTime? ReviewDate { get; set; }

        public DateTime? LastModified_19118105 { get; set; }

        [Range(1,5,ErrorMessage ="Оценката е задължителна")]
        public int Rating { get; set; }
    }
}

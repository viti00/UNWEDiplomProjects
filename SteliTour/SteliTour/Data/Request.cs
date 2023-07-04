using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SteliTour.Data
{
    [Table("Requests", Schema = "19118105")]
    public class Request
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string RequestText { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public IdentityUser? User { get; set; }

        public DateTime? RequestDate { get; set; }

        public DateTime? LastModified_19118105 { get; set; }

        public string? Status { get; set; }

    }
}

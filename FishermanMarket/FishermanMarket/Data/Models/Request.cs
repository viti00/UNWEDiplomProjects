using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace FishermanMarket.Data.Models
{
    [Table("Requests", Schema = "19118062")]
    public class Request
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public IdentityUser? User { get; set; }

        public DateTime LastModified_19118062 { get; set; }

        public DateTime RequestDate { get; set; }

        public string? Status { get; set; }

    }
}

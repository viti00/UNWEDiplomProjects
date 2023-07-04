using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeEssentials.Data
{
    [Table("Reviews", Schema = "19118075")]
    public class Review
    {         
        public Guid Id { get; set; }

        public int Grade { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public IdentityUser? User { get; set; }

        [ForeignKey("Item")]
        public Guid ItemId { get; set; }
        public Item? Item { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? LastModified_19118075 { get; set; }
    }
}

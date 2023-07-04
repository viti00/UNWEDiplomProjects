namespace FitnessProgram.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("UserLikedPosts", Schema = "19118074")]
    public class UserLikedPost
    {
        public string UserId { get; set; }

        [ForeignKey(nameof(Post))]
        public string PostId { get; set; }

        public virtual Post Post { get; set; }

        [Required]
        public DateTime LastModified_19118074 { get; set; }
    }
}

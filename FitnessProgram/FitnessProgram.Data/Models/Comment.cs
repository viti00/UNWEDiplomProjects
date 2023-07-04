﻿namespace FitnessProgram.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using static FitnessProgram.Global.GlobalConstants;

    [Table("Comments", Schema = "19118074")]
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(CommentConstants.MessageMaxLegth)]
        public string Message { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [ForeignKey(nameof(Creator))]
        public string CreatorId { get; set; }

        public virtual User Creator { get; set; }

        [ForeignKey(nameof(Post))]
        public string PostId { get; set; }

        public virtual Post Post { get; set; }

        [Required]
        public DateTime LastModified_19118074 { get; set; }
    }
}

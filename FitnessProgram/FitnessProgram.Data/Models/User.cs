namespace FitnessProgram.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class User : IdentityUser
    {
        [ForeignKey(nameof(ProfilePicture))]
        public int? ProfilePictureId { get; set; }

        public virtual ProfilePhoto? ProfilePicture { get; set; }

        [Required]
        public DateTime LastModified_19118074 { get; set; }
    }
}

namespace FitnessProgram.ViewModels.Post
{
    using FitnessProgram.Data.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using static FitnessProgram.Global.GlobalConstants;

    public class PostFormModel
    {
        [Required]
        [StringLength(PostConstants.TitleMaxLength, MinimumLength = PostConstants.TitleMinLength)]
        public string Title { get; set; }

        [Required]
        [StringLength(PostConstants.TextMaxLegth, MinimumLength = PostConstants.TextMinLegth)]
        public string Text { get; set; }

        public List<PostPhoto>? Photos { get; set; }


        [FromForm]
        [NotMapped]
        public IFormFileCollection? Files { get; set; }
    }
}

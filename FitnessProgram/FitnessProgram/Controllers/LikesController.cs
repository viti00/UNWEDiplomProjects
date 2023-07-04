namespace FitnessProgram.Controllers
{
    using FitnessProgram.Infrastructure;
    using FitnessProgram.Services.LikeService;
    using FitnessProgram.Services.PostServices;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;


    public class LikesController : Controller
    {
        private readonly ILikeService likeService;
        private readonly IPostService postService;

        public LikesController(ILikeService likeService, IPostService postService)
        {
            this.likeService = likeService;
            this.postService = postService;
        }

        [Authorize]
        public IActionResult LikePost(string id)
        {
            if(postService.GetPostById(id) == null)
            {
                return RedirectToAction("Error", "Home", new { message = "This post is not available already" });
            }
            likeService.LikePost(id, User.GetId());

            return Ok();
        }

        [Authorize]
        public IActionResult UnlikePost(string id)
        {
            if (postService.GetPostById(id) == null)
            {
                return RedirectToAction("Error", "Home", new { message = "This post is not available already" });
            }
            likeService.UnlikePost(id, User.GetId());

            return Ok();
        }
    }
}

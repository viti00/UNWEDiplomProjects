namespace FitnessProgram.Controllers
{
    using FitnessProgram.Infrastructure;
    using FitnessProgram.Services.CommentService;
    using FitnessProgram.Services.PostServices;
    using Microsoft.AspNetCore.Mvc;

    public class CommentsController : Controller
    {
        private readonly ICommentService commentService;
        private readonly IPostService postService;

        public CommentsController(ICommentService commentService, IPostService postService)
        {
            this.commentService = commentService;
            this.postService = postService;
        }

        public IActionResult Comment(string id,string message)
        {
            if (postService.GetPostById(id) == null)
            {
                return RedirectToAction("Error", "Home", new { message = "This post is not available already" });
            }
            commentService.Comment(id, message, User.GetId());

            return Ok();

        }

        public string GetMessage(int id) 
            => commentService.GetMessage(id);

        public IActionResult Edit(int id, string message)
        {
            if(commentService.GetCommentById(id) == null)
            {
                return RedirectToAction("Error", "Home", new { message = "This comment is not available already" });
            }
            commentService.Edit(id, message);
            return Ok();
        }

        public IActionResult Delete(int id)
        {
            if (commentService.GetCommentById(id) == null)
            {
                return RedirectToAction("Error", "Home", new { message = "This comment is not available already" });
            }
            commentService.Delete(id);

            return Ok();
        }

        public int CommentsCount(string id)
            => commentService.GetCommentsCount(id);

    }
}

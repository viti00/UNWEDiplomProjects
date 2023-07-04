namespace FitnessProgram.Services.CommentService
{
    using FitnessProgram.Data;
    using FitnessProgram.Data.Models;
    using FitnessProgram.ViewModels.Comment;
    using System.Collections.Generic;
    using static FitnessProgram.Services.SharedMethods;

    public class CommentService : ICommentService
    {
        private readonly FitnessProgramDbContext context;

        public CommentService(FitnessProgramDbContext context) 
            => this.context = context;

        public void Comment(string postId, string message, string userId)
        {
            try
            {
                if (!string.IsNullOrEmpty(postId) && !string.IsNullOrEmpty(message) && !string.IsNullOrEmpty(userId))
                {
                    var comment = new Comment
                    {
                        Message = message,
                        CreatedOn = DateTime.Now,
                        PostId = postId,
                        CreatorId = userId,
                        LastModified_19118074 = DateTime.Now

                    };

                    context.log_19118074.Add(CreateLog("Comments", "Insert"));
                    context.Comments.Add(comment);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
        }

        public void Delete(int commentId)
        {
            try
            {
                var comment = GetCommentById(commentId);

                if (comment != null)
                {
                    context.log_19118074.Add(CreateLog("Comments", "Update"));
                    context.Comments.Remove(comment);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
        }

        public void Edit(int commentId, string message)
        {
            try
            {
                var comment = GetCommentById(commentId);

                if (comment != null)
                {
                    comment.Message = message;
                    comment.LastModified_19118074 = DateTime.Now;

                    context.log_19118074.Add(CreateLog("Comments", "Update"));

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
        }

        public List<Comment> GetAll(string postId)
        {
            try
            {
                var comments = context
                .Comments.Where(x => x.PostId == postId)
                .ToList();

                return comments;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
        }

        public int GetCommentsCount(string postId)
            => context.Comments.Where(x => x.PostId == postId).Count();

        public string GetMessage(int commentId)
        {
            try
            {
                var message = context.Comments
                .Where(x => x.Id == commentId)
                .Select(x => x.Message)
                .FirstOrDefault();

                return message;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
        }

        public CommentViewModel GetNewComment(string postId)
        {
            try
            {
                var comment = context.Comments
               .OrderByDescending(x => x.CreatedOn)
               .Where(x => x.PostId == postId)
               .Select(x => new CommentViewModel
               {
                   Id = x.Id,
                   Message = x.Message,
                   CreatedOn = x.CreatedOn.ToString("MM/dd/yyyy HH:mm"),
                   UserProfilePictire = x.Creator.ProfilePicture != null ? Convert.ToBase64String(x.Creator.ProfilePicture.Bytes) : AnonymousImageConstant.AnonymousImage,
                   UserUsername = x.Creator.UserName,
                   UserId = x.CreatorId
               }).First();

                return comment;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
           
        }

        public Comment GetCommentById(int id)
            => context.Comments.FirstOrDefault(c => c.Id == id);

    }
}

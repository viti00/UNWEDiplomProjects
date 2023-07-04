namespace FitnessProgram.Services.LikeService
{
    using FitnessProgram.Data.Models;
    using FitnessProgram.Data;
    using static FitnessProgram.Services.SharedMethods;

    public class LikeService : ILikeService
    {
        private readonly FitnessProgramDbContext context;

        public LikeService(FitnessProgramDbContext context)
        => this.context = context;

        public List<UserLikedPost> GetAllLikesForPost(string postId)
            => context.UserLikedPosts.Where(x => x.PostId == postId).ToList();

        public string GetLikesCount(string id)
        {
            try
            {
                var likesCount = context.Posts
                            .Where(x => x.Id == id)
                            .Select(x => x.Likes.Count())
                            .FirstOrDefault();

                return likesCount.ToString();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
        }

        public void LikePost(string postId, string userId)
        {
            try
            {
                var post = context.Posts.FirstOrDefault(x => x.Id == postId);

                if (post != null && userId != null)
                {
                    var like = new UserLikedPost
                    {
                        UserId = userId,
                        PostId = postId,
                        LastModified_19118074 = DateTime.Now
                    };

                    context.log_19118074.Add(CreateLog("UserLikedPosts", "Insert"));

                    context.UserLikedPosts.Add(like);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
        }

        public void UnlikePost(string postId, string userId)
        {
            try
            {
                var like = context.UserLikedPosts
               .Where(x => x.PostId == postId && x.UserId == userId)
               .FirstOrDefault();

                if (like != null)
                {
                    context.log_19118074.Add(CreateLog("UserLikedPosts", "Update"));
                    context.UserLikedPosts.Remove(like);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
           
        }
    }
}

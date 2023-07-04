namespace FitnessProgram.Services.PostServices
{
    using FitnessProgram.Data;
    using FitnessProgram.Data.Models;
    using FitnessProgram.ViewModels.Comment;
    using FitnessProgram.ViewModels.Post;
    using FitnessProgram.Services.CommentService;
    using FitnessProgram.Services.LikeService;
    using Microsoft.Extensions.Caching.Memory;
    using static SharedMethods;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Mvc;
    using System.Net;

    public class PostService : IPostService
    {
        private readonly FitnessProgramDbContext context;
        private readonly ICommentService commentService;
        private readonly ILikeService likeService;
        private readonly IMemoryCache cache;

        public PostService(FitnessProgramDbContext context,
                            ICommentService commentService,
                            ILikeService likeService,
                            IMemoryCache cache)
        {
            this.context = context;
            this.commentService = commentService;
            this.likeService = likeService;
            this.cache = cache;
        }
        public void Create(PostFormModel model, string creatorId)
        {
            try
            {
                var photos = CreatePhotos(model.Files);

                var post = new Post
                {
                    Title = model.Title,
                    Photos = photos,
                    Text = model.Text,
                    CreatedOn = DateTime.Now,
                    Likes = new List<UserLikedPost>(),
                    Comments = new List<Comment>(),
                    CreatorId = creatorId,
                    LastModified_19118074 = DateTime.Now
                };

                context.log_19118074.Add(CreateLog("Posts", "Insert"));
                context.Posts.Add(post);
                context.SaveChanges();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public AllPostsQueryModel GetMy(string userId, int currPage, int postPerPage, AllPostsQueryModel query)
        {
            try
            {
                var postsAll = context.Posts
                                  .Where(x => x.CreatorId == userId)
                                  .Include(x => x.Photos)
                                  .Include(x => x.Likes)
                                  .Include(x => x.Comments)
                                  .ToList();

                int totalPosts = postsAll.Count();

                int maxPage = CalcMaxPage(totalPosts, postPerPage);
                currPage = GetCurrPage(currPage, ref maxPage);

                if (!string.IsNullOrWhiteSpace(query.SearchTerm))
                {
                    postsAll = Search(postsAll, query.SearchTerm);
                }

                postsAll = Sort(postsAll, query.Sorting);

                var myPosts = CreateViewModel(postsAll, currPage, postPerPage);

                var result = CreateModel(myPosts, currPage, maxPage, query.SearchTerm, query.Sorting);

                return result;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public AllPostsQueryModel GetAll(int currPage, int postPerPage, AllPostsQueryModel query, bool isAdministrator)
        {
            try
            {
                int totalPosts;

                const string postsCache = "PostCache";

                List<Post> postsAll;

                List<PostViewModel> currPagePosts;


                if (isAdministrator)
                {
                    postsAll = GetPosts();
                }
                else
                {
                    postsAll = cache.Get<List<Post>>(postsCache);
                    if (postsAll == null)
                    {
                        postsAll = GetPosts();

                        var cacheOptions = new MemoryCacheEntryOptions()
                            .SetAbsoluteExpiration(TimeSpan.FromSeconds(30));

                        cache.Set(postsCache, postsAll, cacheOptions);
                    }
                }

                if (!string.IsNullOrWhiteSpace(query.SearchTerm))
                {
                    postsAll = Search(postsAll, query.SearchTerm);
                }

                postsAll = Sort(postsAll, query.Sorting);

                totalPosts = postsAll.Count();

                var maxPage = CalcMaxPage(totalPosts, postPerPage);

                currPage = GetCurrPage(currPage, ref maxPage);

                currPagePosts = CreateViewModel(postsAll, currPage, postPerPage);

                var result = CreateModel(currPagePosts, currPage, maxPage, query.SearchTerm, query.Sorting);

                return result;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public PostDetailsModel GetPostDetails(string postId, string userId)
        {
            try
            {
                var alreadyLiked = context.UserLikedPosts.Any(x => x.PostId == postId && x.UserId == userId);

                var post = context.Posts
                    .Where(x => x.Id == postId)
                    .Select(x => new PostDetailsModel
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Photos = x.Photos.Select(x => Convert.ToBase64String(x.Bytes)).ToList(),
                        Text = x.Text,
                        CreatedOn = x.CreatedOn.ToString("MM/dd/yyyy HH:mm"),
                        LikesCount = x.Likes.Count(),
                        IsCurrUserLikedPost = alreadyLiked,
                        Comments = x.Comments
                                    .OrderByDescending(x => x.CreatedOn)
                                    .Select(x => new CommentViewModel
                                    {
                                        Id = x.Id,
                                        Message = x.Message,
                                        CreatedOn = x.CreatedOn.ToString("MM/dd/yyyy HH:mm"),
                                        UserProfilePictire = x.Creator.ProfilePicture != null ? Convert.ToBase64String(x.Creator.ProfilePicture.Bytes) : AnonymousImageConstant.AnonymousImage,
                                        UserUsername = x.Creator.UserName,
                                        UserId = x.CreatorId
                                    })
                                    .ToList(),
                        Creator = new UserViewModel
                        {
                            Id = x.CreatorId,
                            ProfilePicture = x.Creator.ProfilePicture != null ? Convert.ToBase64String(x.Creator.ProfilePicture.Bytes) : AnonymousImageConstant.AnonymousImage,
                            Username = x.Creator.UserName,
                        }
                    }).FirstOrDefault();

                return post;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public PostFormModel CreateEditModel(Post post)
        {
            try
            {
                var model = new PostFormModel
                {
                    Title = post.Title,
                    Text = post.Text,
                    Photos = post.Photos.ToList()
                };

                return model;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public void Edit(PostFormModel model, string postId)
        {
            try
            {
                var post = GetPostById(postId);
                if(post != null)
                {
                    var photos = CreatePhotos(model.Files);

                    post.Title = model.Title;
                    post.Text = model.Text;
                    foreach (var photo in photos)
                    {
                        post.Photos.Add(photo);

                        context.log_19118074.Add(CreateLog("PostsPhotos", "Insert"));
                    }
                    context.log_19118074.Add(CreateLog("Posts", "Update"));
                    context.SaveChanges();
                }
                
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public void Delete(Post post)
        {
            try
            {
                context.RemoveRange(post.Photos);

                context.log_19118074.Add(CreateLog("PostPhotos", "Update"));

                var comments = commentService.GetAll(post.Id);

                context.log_19118074.Add(CreateLog("Comments", "Update"));
                context.Comments.RemoveRange(comments);
                context.log_19118074.Add(CreateLog("UserLikedPost", "Update"));
                var likes = likeService.GetAllLikesForPost(post.Id);

                context.UserLikedPosts.RemoveRange(likes);

                context.log_19118074.Add(CreateLog("Posts", "Update"));
                context.Posts.Remove(post);
                context.SaveChanges();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public Post GetPostById(string postId)
            => context.Posts
            .Include(p => p.Photos)
            .FirstOrDefault(x => x.Id == postId);


        private List<PostPhoto> CreatePhotos(IFormFileCollection files)
        {
            try
            {
                List<PostPhoto> photos = new List<PostPhoto>();
                Task.Run(async () =>
                {
                    if (files != null)
                    {
                        foreach (var file in files)
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                await file.CopyToAsync(memoryStream);

                                if (memoryStream.Length < 2097152)
                                {
                                    var newphoto = new PostPhoto()
                                    {
                                        Bytes = memoryStream.ToArray(),
                                        Description = file.FileName,
                                        FileExtension = Path.GetExtension(file.FileName),
                                        Size = file.Length,
                                        LastModified_19118074 = DateTime.Now
                                    };
                                    photos.Add(newphoto);
                                }
                            }
                        }
                    }
                }).GetAwaiter()
                   .GetResult();

                return photos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        private List<Post> Search(List<Post> postsAll, string searchTerm)
            => postsAll
                    .Where(p => p.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    .ToList();

        private List<Post> Sort(List<Post> postsAll, Sorting sorting)
            => postsAll = sorting switch
            {
                Sorting.Default => postsAll.OrderByDescending(x => x.LastModified_19118074).ToList(),
                Sorting.LikesAscending => postsAll.OrderBy(x => x.Likes.Count()).ThenByDescending(x => x.LastModified_19118074).ToList(),
                Sorting.LikesDescending => postsAll.OrderByDescending(x => x.Likes.Count()).ThenByDescending(x => x.LastModified_19118074).ToList(),
                Sorting.CommentsAscending => postsAll.OrderBy(x => x.Comments.Count()).ThenByDescending(x => x.LastModified_19118074).ToList(),
                Sorting.CommentsDescending => postsAll.OrderByDescending(x => x.Comments.Count()).ThenByDescending(x => x.LastModified_19118074).ToList(),
                Sorting.DateAscending => postsAll.OrderBy(x => x.LastModified_19118074).ToList(),
                _ => postsAll.OrderByDescending(x => x.LastModified_19118074).ToList()
            };

        private List<PostViewModel> CreateViewModel(List<Post> postsAll, int currPage, int postPerPage)
        {
            try
            {
                var posts = postsAll
                .Skip((currPage - 1) * postPerPage)
                .Take(postPerPage).ToList()
                .Select(x => new PostViewModel
                {
                    PostId = x.Id,
                    Title = x.Title,
                    Photos = x.Photos.Select(x => Convert.ToBase64String(x.Bytes)).ToList(),
                    LikesCount = x.Likes.Count(),
                    CommentsCount = x.Comments.Count(),
                    CreatedOn = x.CreatedOn,

                })
                .ToList();

                return posts;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        private AllPostsQueryModel CreateModel
            (List<PostViewModel> posts, int currPage, int maxPage, string serchTerm, Sorting sorting)
            => new AllPostsQueryModel
            {
                Posts = posts,
                CurrentPage = currPage,
                MaxPage = maxPage,
                SearchTerm = serchTerm,
                Sorting = sorting
            };

        private List<Post> GetPosts()
            => context.Posts
               .Include(l => l.Likes)
               .Include(c => c.Comments)
               .Include(p => p.Photos)
               .ToList();

        public bool RemovePhoto(string postId, int photoId)
        {
            try
            {
                var photo = context.PostPhotos.FirstOrDefault(x => x.PostId == postId && x.Id == photoId);

                if (photo != null)
                {
                    context.log_19118074.Add(CreateLog("PostPhotos", "Update"));
                    context.PostPhotos.Remove(photo);
                    context.SaveChanges();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
        }

        public bool RemoveAllPhotos(string postId)
        {
            try
            {
                var photos = context.PostPhotos.Where(x => x.PostId == postId).ToList();

                context.log_19118074.Add(CreateLog("PostPhotos", "Update"));
                context.PostPhotos.RemoveRange(photos);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
           
        }
    }
}

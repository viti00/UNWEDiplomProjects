namespace FitnessProgram.Services.BestResultService
{
    using FitnessProgram.Data;
    using FitnessProgram.Data.Models;
    using FitnessProgram.ViewModels.BestResult;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;
    using static SharedMethods;

    public class BestResultService : IBestResultService
    {
        private readonly FitnessProgramDbContext context;
        private readonly IMemoryCache cache;

        const string typeBefore = "Before";
        const string typeAfter = "After";

        public BestResultService(FitnessProgramDbContext context, IMemoryCache cache)
        {
            this.context = context;
            this.cache = cache;
        }


        public AllBestResultsQueryModel GetAll(int currPage, int postPerPage, AllBestResultsQueryModel query, bool isAdministrator)
        {
            try
            {
                const string bestResultCache = "BestResultCache";

                int totalPosts;

                List<BestResultViewModel> currPageBestResults;

                List<BestResult> bestResults;

                if (isAdministrator)
                {
                    bestResults = GetBestResults();
                }
                else
                {
                    bestResults = cache.Get<List<BestResult>>(bestResultCache);
                    if (bestResults == null)
                    {
                        bestResults = GetBestResults();

                        var cacheOptions = new MemoryCacheEntryOptions()
                            .SetAbsoluteExpiration(TimeSpan.FromSeconds(30));

                        cache.Set(bestResultCache, bestResults, cacheOptions);
                    }
                }

                if (!string.IsNullOrWhiteSpace(query.SearchTerm))
                {
                    bestResults = bestResults
                        .Where(p => p.Story.Contains(query.SearchTerm, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                bestResults = query.Sorting switch
                {
                    Sorting.Default => bestResults.OrderByDescending(x => x.LastModified_19118074).ToList(),
                    Sorting.DateAscending => bestResults.OrderBy(x => x.LastModified_19118074).ToList(),
                    _ => bestResults.OrderByDescending(x => x.LastModified_19118074).ToList()
                };

                totalPosts = bestResults.Count();

                var maxPage = CalcMaxPage(totalPosts, postPerPage);

                currPage = GetCurrPage(currPage, ref maxPage);

                currPageBestResults = bestResults
                .Skip((currPage - 1) * postPerPage)
                .Take(postPerPage).ToList()
                .Select(x => new BestResultViewModel
                {
                    Id = x.Id,
                    BeforePhotos = x.Photos.Where(x => x.PhotoType == typeBefore).Select(x => Convert.ToBase64String(x.Bytes)).ToList(),
                    AfterPhotos = x.Photos.Where(x => x.PhotoType == typeAfter).Select(x => Convert.ToBase64String(x.Bytes)).ToList(),
                    Story = x.Story,
                    CreatedOn = x.CreatedOn

                })
               .ToList();

                var result = new AllBestResultsQueryModel
                {
                    BestResults = currPageBestResults,
                    CurrentPage = currPage,
                    MaxPage = maxPage,
                    SearchTerm = query.SearchTerm,
                    Sorting = query.Sorting
                };

                return result;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
        }

        public BestResultDetailsModel GetDetails(int bestresultId)
        {
            try
            {
                var bestResult = GetBestResultById(bestresultId);

                if (bestResult == null)
                {
                    return null;
                }

                var model = new BestResultDetailsModel
                {
                    Id = bestResult.Id,
                    Photos = bestResult.Photos.Select(x => Convert.ToBase64String(x.Bytes)).ToList(),
                    CreatedOn = bestResult.CreatedOn.ToString("MM/dd/yyyy HH:mm"),
                    Story = bestResult.Story,
                    RowVersion = bestResult.RowVersion
                };

                return model;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
           
        }


        public void AddBestResult(BestResultFormModel model)
        {
            try
            {
                var photos = PrepareCreatePhotos(model.BeforeFiles, model.AfterFiles);

                var bestResult = new BestResult
                {
                    Photos = photos,
                    CreatedOn = DateTime.Now,
                    Story = model.Story,
                    LastModified_19118074 = DateTime.Now,
                };

                context.log_19118074.Add(CreateLog("BestResults", "Insert"));
                if (photos.Count > 0)
                {
                    context.log_19118074.Add(CreateLog("BestResultsPhotos", "Insert"));
                }


                context.BestResults.Add(bestResult);
                context.SaveChanges();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
        }

        public BestResultFormModel CreateEditModel(int bestResultId)
        {
            try
            {
                var bestResult = GetBestResultById(bestResultId);

                if (bestResult == null)
                {
                    return null;
                }

                var editModel = new BestResultFormModel
                {
                    Story = bestResult.Story,
                    Photos = bestResult.Photos,
                    RowVersion = bestResult.RowVersion
                };

                return editModel;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
        }

        public bool EditBestResult(int bestResultId,BestResultFormModel model, int startRowVersion)
        {
            try
            {
                var bestResult = GetBestResultById(bestResultId);

                var photos = PrepareCreatePhotos(model.BeforeFiles, model.AfterFiles);
                foreach (var photo in photos)
                {
                    bestResult.Photos.Add(photo);
                    context.log_19118074.Add(CreateLog("BestResultPhotos", "Insert"));
                }
                bestResult.Story = model.Story;
                bestResult.LastModified_19118074 = DateTime.Now;

                context.log_19118074.Add(CreateLog("BestResults", "Update"));
                var endRowVersion = GetBestResultById(bestResultId).RowVersion;
                if(startRowVersion != endRowVersion)
                {
                    return false;
                }
                bestResult.RowVersion++;
                context.SaveChanges();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            return true;
            
        }

        public void DeleteBestResult(BestResult bestResult)
        {
            try
            {
                if (bestResult != null)
                {
                    context.log_19118074.Add(CreateLog("BestResults", "Update"));

                    context.BestResultPhotos.RemoveRange(bestResult.Photos);

                    context.log_19118074.Add(CreateLog("BestResultPhotos", "Update"));
                    context.BestResults.Remove(bestResult);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
        }

        public BestResult GetBestResultById(int id)
            => context.BestResults
            .Include(br=> br.Photos)
            .FirstOrDefault(x => x.Id == id);


        private List<BestResultPhoto> PrepareCreatePhotos(IFormFileCollection beforeFiles, IFormFileCollection afterFiles)
        {
            try
            {
                List<BestResultPhoto> photos = new List<BestResultPhoto>();
                Parallel.Invoke(() =>
                {
                    CreatePhotos(beforeFiles, typeBefore, photos);
                },
                () =>
                {
                    CreatePhotos(afterFiles, typeAfter, photos);
                });

                return photos;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
           
        }

        private List<BestResultPhoto> CreatePhotos(IFormFileCollection files, string type, List<BestResultPhoto> photos)
        {
            try
            {
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
                                    var newphoto = new BestResultPhoto()
                                    {
                                        Bytes = memoryStream.ToArray(),
                                        Description = file.FileName,
                                        FileExtension = Path.GetExtension(file.FileName),
                                        Size = file.Length,
                                        PhotoType = type,
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

        private List<BestResult> GetBestResults()
            => context.BestResults
               .Include(br => br.Photos)
               .ToList();

        public bool RemovePhoto(int postId, int photoId)
        {
            try
            {
                var photo = context.BestResultPhotos.FirstOrDefault(x => x.BestResultId == postId && x.Id == photoId);

                if (photo != null)
                {
                    context.log_19118074.Add(CreateLog("BestResultsPhotos", "Update"));
                    context.BestResultPhotos.Remove(photo);
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

        public bool RemoveAllPhotos(int postId)
        {
            try
            {
                var photos = context.BestResultPhotos.Where(x => x.BestResultId == postId).ToList();
                if(photos.Count > 0)
                {
                    context.log_19118074.Add(CreateLog("BestResultsPhotos", "Update"));
                    context.BestResultPhotos.RemoveRange(photos);
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
    }
}

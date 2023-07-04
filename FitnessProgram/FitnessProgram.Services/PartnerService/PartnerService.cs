namespace FitnessProgram.Services.PartnerService
{
    using FitnessProgram.Data;
    using FitnessProgram.Data.Models;
    using FitnessProgram.ViewModels.Partner;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;
    using static SharedMethods;

    public class PartnerService : IPartnerService
    {
        private readonly FitnessProgramDbContext context;
        private readonly IMemoryCache cache;

        public PartnerService(FitnessProgramDbContext context, IMemoryCache cache)
        {
            this.context = context;
            this.cache = cache;
        }

        public AllPartnersQueryModel GetAll(int currPage, int postPerPage, AllPartnersQueryModel query,bool isAdministrator)
        {
            try
            {
                const string partnersCache = "PartnersCache";

                int totalPosts;

                List<Partner> partners;

                List<PartnersViewModel> currPagePartners;

                if (isAdministrator)
                {
                    partners = GetPartners();
                }
                else
                {
                    partners = cache.Get<List<Partner>>(partnersCache);
                    if (partners == null)
                    {
                        partners = GetPartners();

                        var cacheOptions = new MemoryCacheEntryOptions()
                            .SetAbsoluteExpiration(TimeSpan.FromSeconds(30));

                        cache.Set(partnersCache, partners, cacheOptions);
                    }
                }

                if (!string.IsNullOrWhiteSpace(query.SearchTerm))
                {
                    partners = partners
                        .Where(p => p.Name.Contains(query.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                               p.Description.Contains(query.SearchTerm, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                partners = query.Sorting switch
                {
                    Sorting.Default => partners.OrderByDescending(x => x.LastModified_19118074).ToList(),
                    Sorting.DateAscending => partners.OrderBy(x => x.LastModified_19118074).ToList(),
                    _ => partners.OrderByDescending(x => x.LastModified_19118074).ToList()
                };

                totalPosts = partners.Count();

                var maxPage = CalcMaxPage(totalPosts, postPerPage);

                currPage = GetCurrPage(currPage, ref maxPage);

                currPagePartners = partners
                .Skip((currPage - 1) * postPerPage)
                .Take(postPerPage).ToList()
                .Select(x => new PartnersViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Photo = x.Photo != null ? Convert.ToBase64String(x.Photo.Bytes) : null,
                    Url = x.Url,
                    PromoCode = x.PromoCode,
                    Description = x.Description,
                    CreatedOn = x.CreatedOn,
                    RowVersion = x.RowVersion,
                    LastModified_19118074 = x.LastModified_19118074
                })
               .ToList();

                var result = new AllPartnersQueryModel
                {
                    Partners = currPagePartners,
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

        public void AddPartner(PartnerFormModel model)
        {
            try
            {
                var photo = CreatePhoto(model.File);

                if (photo != null)
                {
                    context.log_19118074.Add(CreateLog("PartnerPhotos", "Insert"));
                }

                var partner = new Partner
                {
                    Name = model.Name,
                    Description = model.Description,
                    Photo = photo,
                    Url = model.Url,
                    PromoCode = model.PromoCode,
                    CreatedOn = DateTime.Now,
                    LastModified_19118074 = DateTime.Now
                };

                context.log_19118074.Add(CreateLog("Partners", "Insert"));
                context.Partners.Add(partner);
                context.SaveChanges();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
           
        }

        public PartnerFormModel CreateEditModel(int partnerId)
        {
            try
            {
                var partner = GetPartnerById(partnerId);

                if (partner == null)
                {
                    return null;
                }

                var model = new PartnerFormModel
                {
                    Name = partner.Name,
                    Description = partner.Description,
                    Url = partner.Url,
                    PromoCode = partner.PromoCode,
                    PartnerPhoto = partner.Photo,
                    RowVersion = partner.RowVersion
                };

                return model;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
           
        }

        public bool EditPartner(int partnerId, PartnerFormModel model, int startRowVersion)
        {
            try
            {
                var partner = GetPartnerById(partnerId);
                

                if (partner != null)
                {
                    var photo = CreatePhoto(model.File);
                    if (photo != null)
                    {
                        if(partner.Photo != null)
                        {
                            var photoForDelete = context.PartnerPhotos.FirstOrDefault(x => x.Id == partner.Photo.Id);

                            context.log_19118074.Add(CreateLog("PartnerPhotos", "Update"));

                            context.PartnerPhotos.Remove(photoForDelete);
                        }
                        

                        context.log_19118074.Add(CreateLog("PartnerPhotos", "Insert"));

                        partner.Photo = photo;

                    }


                    partner.Name = model.Name;
                    partner.Description = model.Description;
                    partner.LastModified_19118074 = DateTime.Now;

                    partner.Url = model.Url;
                    partner.PromoCode = model.PromoCode;
                    

                    context.log_19118074.Add(CreateLog("Partners", "Update"));

                    var endRowVersion = GetPartnerById(partnerId).RowVersion;
                    if (startRowVersion!= endRowVersion)
                    {
                        return false;
                    }
                    partner.RowVersion++;
                    context.SaveChanges();

                    
                }
                
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            return true;

        }
        public void DeletePartner(Partner partner)
        {
            try
            {
                if (partner != null)
                {
                    if (partner.Photo != null)
                    {
                        context.PartnerPhotos.Remove(partner.Photo);

                        context.log_19118074.Add(CreateLog("PartnerPhotos", "Update"));
                    }

                    context.log_19118074.Add(CreateLog("Partners", "Update"));
                    context.Partners.Remove(partner);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
           
        }

        public Partner GetPartnerById(int id)
            => context.Partners
            .Include(p=> p.Photo)
            .FirstOrDefault(x => x.Id == id);

        private PartnerPhoto CreatePhoto(IFormFile file)
        {
            try
            {
                PartnerPhoto photo = null;

                if (file != null)
                {
                    Task.Run(async () =>
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await file.CopyToAsync(memoryStream);

                            if (memoryStream.Length < 2097152)
                            {
                                var newPhoto = new PartnerPhoto()
                                {
                                    Bytes = memoryStream.ToArray(),
                                    Description = file.FileName,
                                    FileExtension = Path.GetExtension(file.FileName),
                                    Size = file.Length,
                                    LastModified_19118074 = DateTime.Now
                                };
                                photo = newPhoto;
                            }

                        }
                    }).GetAwaiter()
                   .GetResult();
                }

                return photo;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
        }

        public bool RemovePhoto(int postId, int photoId)
        {
            try
            {
                var photo = context.PartnerPhotos.FirstOrDefault(x => x.PartnerId == postId && x.Id == photoId);

                if (photo != null)
                {
                    context.log_19118074.Add(CreateLog("PartnerPhotos", "Update"));
                    context.PartnerPhotos.Remove(photo);
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

        private List<Partner> GetPartners()
            => context.Partners
               .Include(p => p.Photo)
               .ToList();
    }
}

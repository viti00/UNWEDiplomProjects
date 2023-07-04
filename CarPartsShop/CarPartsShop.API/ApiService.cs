using CarPartsShop.API.DTO;
using CarPartsShop.Data;
using CarPartsShop.Data.Models;
using CarPartsShop.Models.FormModels;
using CarPartsShop.Models.ViewModels;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.EntityFrameworkCore;
using static CarPartsShop.NoImage;

namespace CarPartsShop.API
{
    public class ApiService
    {
        private readonly CarPartsShopDbContext context;

        public ApiService(CarPartsShopDbContext context)
        {
            this.context = context;
        }

        public async Task<List<PartDTO>> GetAll()
        {
            var parts = await context.Parts
                .Include(x => x.PartCondtion)
                .Include(x => x.PartType)
                .Include(x => x.PartImages)
                .Select(x => new PartDTO
                {
                    Id = x.Id,
                    Price = x.PartPrice,
                    Name = x.Name,
                    Condition = x.PartCondtion.Condition,
                    Image = x.PartImages.Count > 0 ? Convert.ToBase64String(x.PartImages.First().Bytes) : NoImageString,
                    Type = x.PartType.Type,
                    LastModified_19118067 = x.LastModified_19118067
                }).ToListAsync();

            return parts;
        }
        public async Task<List<PartDTO>> GetById(string id)
        {
            var parts = await context.Parts
                .Where(x=> x.Id == id)
                .Include(x => x.PartCondtion)
                .Include(x => x.PartType)
                .Include(x => x.PartImages)
                .Select(x => new PartDTO
                {
                    Id = x.Id,
                    Price = x.PartPrice,
                    Name = x.Name,
                    Condition = x.PartCondtion.Condition,
                    Image = x.PartImages.Count > 0 ? Convert.ToBase64String(x.PartImages.First().Bytes) : NoImageString,
                    Type = x.PartType.Type,
                    LastModified_19118067 = x.LastModified_19118067
                }).ToListAsync();

            return parts;
        }

        public async Task<Part> CreatePart(CreatePartDTO model)
        {
            var images = CreatePartImages(model.Files);

            var part = new Part
            {
                Name = model.Name,
                Description = model.Description,
                StockQnt = model.StockQnt,
                PartCondtion = await context.PartCondtions.FirstOrDefaultAsync(x => x.Id == model.PartCondition),
                PartType = await context.PartTypes.FirstOrDefaultAsync(x => x.Id == model.PartType),
                PartPrice = model.PartPrice,
                PartImages = images,
                LastModified_19118067 = DateTime.Now
            };
            await context.Parts.AddAsync(part);
            await context.SaveChangesAsync();

            return part;
        }

        public async Task<Part> EditPart(CreatePartDTO model, string id)
        {
            var part = await context.Parts
                .Include(x => x.PartCondtion)
                .Include(x => x.PartType)
                .Include(x => x.PartImages)
                .FirstOrDefaultAsync(x => x.Id == id);

            var images = CreatePartImages(model.Files);

            part.Name = model.Name;
            part.Description = model.Description;
            part.PartCondtion = await context.PartCondtions.FirstOrDefaultAsync(x => x.Id == model.PartCondition);
            part.PartType = await context.PartTypes.FirstOrDefaultAsync(x => x.Id == model.PartType);
            part.PartPrice = model.PartPrice;
            part.StockQnt = model.StockQnt;
            part.LastModified_19118067 = DateTime.Now;
            foreach (var image in images)
            {
                part.PartImages.Add(image);
            }
            context.SaveChanges();
            return part;
        }

        public async Task<bool> DeletePart(string partId)
        {
            var part = await context.Parts
                .Include(x => x.PartCondtion)
                .Include(x => x.PartType)
                .Include(x => x.PartImages)
                .FirstOrDefaultAsync(x => x.Id == partId);

            if(part == null)
            {
                return false;
            }

            context.Parts.Remove(part);
            await context.SaveChangesAsync();

            return true;
        }

        private List<PartImage> CreatePartImages(IFormFileCollection files)
        {
            List<PartImage> images = new List<PartImage>();
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
                                var image = new PartImage()
                                {
                                    Bytes = memoryStream.ToArray(),
                                    Description = file.FileName,
                                    FileExtension = Path.GetExtension(file.FileName),
                                    Size = file.Length,
                                    LastModified_19118067 = DateTime.Now
                                };
                                images.Add(image);
                            }
                        }
                    }
                }
            }).GetAwaiter()
               .GetResult();

            return images;
        }
    }
}

using CarPartsShop.Data;
using CarPartsShop.Models.FormModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using CarPartsShop.Data.Models;
using CarPartsShop.Models.ViewModels;
using static CarPartsShop.NoImage;
using Microsoft.EntityFrameworkCore;

namespace CarPartsShop.Services.PartService
{
    public class PartService : IPartService
    {
        public readonly CarPartsShopDbContext context;
        public PartService(CarPartsShopDbContext context)
        {
            this.context = context;
        }

        public QueryModelParts GetAllParts(QueryModelParts query,string condition)
        {

            var partsQueriable = context.Parts
                               .Include(x => x.PartImages)
                               .Include(x => x.PartCondtion)
                               .Include(x => x.PartType).AsQueryable();
            if(condition != null)
            {
                partsQueriable = partsQueriable.Where(x => x.PartCondtion.Condition == condition).AsQueryable();
            }

            var parts = partsQueriable.Select(x => new PartsAllViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.PartPrice,
                Condition = x.PartCondtion.Condition,
                Image = x.PartImages.Count > 0 ? Convert.ToBase64String(x.PartImages.First().Bytes) : NoImageString,
                Type = x.PartType.Type
            }).ToList();

            
            

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                parts = parts.Where(x => x.Name.Contains(query.Search, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            parts = query.Sorting switch
            {
                Sorting.Default => parts.OrderByDescending(x => x.LastModified_19118067).ToList(),
                Sorting.PriceAscending => parts.OrderBy(x => x.Price).ThenByDescending(x => x.LastModified_19118067).ToList(),
                Sorting.PriceDescending => parts.OrderByDescending(x => x.Price).ThenByDescending(x => x.LastModified_19118067).ToList(),
                _=> parts.OrderByDescending(x => x.LastModified_19118067).ToList(),
            };

            parts = query.Filter switch
            {
                Filter.None => parts.ToList(),
                Filter.BrakingSystem => parts.Where(x=> x.Type == "Спирачна система").ToList(),
                Filter.Suspension => parts.Where(x => x.Type == "Окачване").ToList(),
                Filter.EngineParts=> parts.Where(x => x.Type == "Кормилна система").ToList(),
                Filter.SteeringSystem=> parts.Where(x => x.Type == "Части за двигател").ToList(),
                Filter.CylinderBlockParts => parts.Where(x => x.Type == "Части за цилиндров блок").ToList(),
                Filter.Garnishes => parts.Where(x => x.Type == "Гарнитури").ToList(),
                Filter.SensorsAndSensors => parts.Where(x => x.Type == "Датчици и сенозри").ToList(),
                Filter.FuelSystem => parts.Where(x => x.Type == "Горивна система").ToList(),
                Filter.MufflersAndPots => parts.Where(x => x.Type == "Ауспуси и гърнета").ToList(),
                Filter.Tuning => parts.Where(x => x.Type == "Тунинг").ToList(),
                Filter.StartingSystem => parts.Where(x => x.Type == "Стартова система").ToList(),
                Filter.Lights => parts.Where(x => x.Type == "Светлини").ToList(),
                Filter.AutoAccessories => parts.Where(x => x.Type == "Автоаксесоари").ToList(),
                _ => parts.ToList()
            };

           

            int totalParts = parts.Count();

            int lastPage = CalcLastPage(totalParts, query.PageElements);
            query.CurrentPage = GetCurrPage(query.CurrentPage, ref lastPage);

            parts = parts.Skip((query.CurrentPage - 1) * query.PageElements)
           .Take(query.PageElements).ToList();

            var result = new QueryModelParts
            {
                Parts = parts.ToList(),
                CurrentPage = query.CurrentPage,
                LastPage = lastPage,
                Filter = query.Filter,
                Sorting = query.Sorting,
                Search = query.Search
            };

            return result;
        }

        public void CreatePart(PartsFormModel model)
        {
            var images = CreatePartImages(model.Files);

            var part = new Part
            {
                Name = model.Name,
                Description = model.Description,
                StockQnt = model.StockQnt,
                PartCondtion = context.PartCondtions.FirstOrDefault(x => x.Id == model.PartCondition),
                PartType = context.PartTypes.FirstOrDefault(x => x.Id == model.PartType),
                PartPrice = model.PartPrice,
                PartImages = images,
                LastModified_19118067 = DateTime.Now
            };

            var log = new log_19118067
            {
                TableName = "Parts",
                OperationType = "Insert",
                OperationCreateTime = DateTime.Now
            };

            context.Parts.Add(part);
            context.log_19118067.Add(log);
            context.SaveChanges();
        }

        public void EditPart(PartsFormModel model, Part part)
        {
            var images = CreatePartImages(model.Files);

            part.Name = model.Name;
            part.Description = model.Description;
            part.PartCondtion = context.PartCondtions.FirstOrDefault(x => x.Id == model.PartCondition);
            part.PartType = context.PartTypes.FirstOrDefault(x => x.Id == model.PartType);
            part.PartPrice = model.PartPrice;
            part.StockQnt = model.StockQnt;
            part.LastModified_19118067 = DateTime.Now;
            foreach (var image in images)
            {
                part.PartImages.Add(image);
            }

            var log = new log_19118067
            {
                TableName = "Parts",
                OperationType = "Update",
                OperationCreateTime = DateTime.Now
            };
            context.log_19118067.Add(log);
            context.SaveChanges();
        }

        public void DeletePart(string partId)
        {
            var part = GetPartById(partId);

            var log = new log_19118067
            {
                TableName = "Parts",
                OperationType = "Delete",
                OperationCreateTime = DateTime.Now
            };
            context.log_19118067.Add(log);

            context.Parts.Remove(part);
            context.SaveChanges();
        }

        public PartsFormModel GetFormModel(Part part)
        {
            var formModel = new PartsFormModel
            {
                Name = part.Name,
                Description = part.Description,
                StockQnt = part.StockQnt,
                PartPrice = part.PartPrice,
                PartCondition = part.PartConditionId,
                PartType = part.PartTypeId
            };

            return formModel;
        }

        public Part GetPartById(string partId)
        {
            return context.Parts
                .Include(x=> x.PartImages)
                .Include(x=> x.PartType)
                .Include(x=> x.PartCondtion)
                .FirstOrDefault(x => x.Id == partId);
        }

        public ViewDataDictionary GetViewData(ViewDataDictionary vd)
        {
            vd["ConditionId"] = new SelectList(context.PartCondtions, "Id", "Condition");
            vd["TypeId"] = new SelectList(context.PartTypes, "Id", "Type");
            return vd;
        }

        public void ValidatePart(PartsFormModel model, ModelStateDictionary ms)
        {
            if(!context.PartCondtions.Any(x=> x.Id == model.PartCondition))
            {
                ms.AddModelError("Condition", "Полето е задължително!");
            }
            if (!context.PartTypes.Any(x => x.Id == model.PartType))
            {
                ms.AddModelError("Type", "Полето е задължително!");
            }
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

        private int CalcLastPage(int totalParts, int pageElements)
        {
            var lastPage = (int)Math.Ceiling((double)totalParts / pageElements);

            return lastPage;
        }

        private int GetCurrPage(int currPage, ref int lastPage)
        {
            if (currPage > lastPage)
            {
                if (lastPage == 0)
                {
                    lastPage = 1;
                }
                currPage = lastPage;
            }
            if (currPage == 0)
            {
                currPage = 1;
            }

            return currPage;
        }
    }
}

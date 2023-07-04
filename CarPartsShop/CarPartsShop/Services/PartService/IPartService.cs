using CarPartsShop.Data.Models;
using CarPartsShop.Models.FormModels;
using CarPartsShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace CarPartsShop.Services.PartService
{
    public interface IPartService
    {
        public ViewDataDictionary GetViewData(ViewDataDictionary vd);

        public QueryModelParts GetAllParts(QueryModelParts query, string condition);

        public void ValidatePart(PartsFormModel model, ModelStateDictionary ms);

        public void CreatePart(PartsFormModel model);

        public Part GetPartById(string partId);

        public PartsFormModel GetFormModel(Part part);

        public void EditPart(PartsFormModel model, Part part);

        public void DeletePart(string partId);
    }
}

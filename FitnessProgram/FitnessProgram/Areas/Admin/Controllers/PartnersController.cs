namespace FitnessProgram.Areas.Admin.Controllers
{
    using FitnessProgram.ViewModels.Partner;
    using FitnessProgram.Infrastructure;
    using FitnessProgram.Services.PartnerService;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using static WebConstants;
    using FitnessProgram.Services.BestResultService;
    using FitnessProgram.Data.Models;

    [Area(AdminConstants.AreaName)]
    public class PartnersController : Controller
    {
        private readonly IPartnerService partnerService;

        public PartnersController(IPartnerService partnerService) 
            => this.partnerService = partnerService;

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Add()
        {
            return View();
        }

        [Authorize(Roles = AdministratorRoleName)]
        [HttpPost]
        public IActionResult Add(PartnerFormModel model)
        {
            if(model.File == null)
            {
                ModelState.AddModelError("File", "The field is required");
            }
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(x => x.Errors.Select(x => x.ErrorMessage)).ToList();
                return View(model);
            }

            partnerService.AddPartner(model);

            return RedirectToAction("All", "Partners", new { area = "" });
        }

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Edit(int id)
        {
            var partner = partnerService.CreateEditModel(id);

            if(partner == null)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = "This post is not available already" });
            }

            return View(partner);
        }

        [Authorize(Roles = AdministratorRoleName)]
        [HttpPost]
        public IActionResult Edit(int id, PartnerFormModel model, int rowVersion)
        {
            var startRowVersion = rowVersion;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var isSuccess = partnerService.EditPartner(id, model, startRowVersion);
            if (isSuccess)
            {
                return RedirectToAction("All","Partners", new {area=""});
            }
            else
            {
                ModelState.AddModelError("Already", "This record have been updated before your update");
                return View(model);
            }

            
        }

        [Authorize(Roles =AdministratorRoleName)]
        public IActionResult Delete(int id)
        {
            var partner = partnerService.GetPartnerById(id);

            if (!User.IsAdministrator())
            {
                return Unauthorized();
            }

            if (partner == null)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = "This post is not available already" });
            }

            partnerService.DeletePartner(partner);

            return RedirectToAction("All", "Partners", new { area = "" });
        }

        [Authorize(Roles = AdministratorRoleName)]
        [HttpPost]
        public bool RemovePhoto(int postId, int id)
        {
            return partnerService.RemovePhoto(postId, id);
        }
    }
}

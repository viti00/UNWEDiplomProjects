namespace FitnessProgram.Areas.Admin.Controllers
{
    using FitnessProgram.ViewModels.BestResult;
    using FitnessProgram.Infrastructure;
    using FitnessProgram.Services.BestResultService;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using static WebConstants;
    using FitnessProgram.Data.Models;

    [Area(AdminConstants.AreaName)]
    public class BestResultsController : Controller
    {
        private readonly IBestResultService bestResultService;

        public BestResultsController(IBestResultService bestResultService)
            => this.bestResultService = bestResultService;

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Add()
        {
            return View();
        }

        [Authorize(Roles = AdministratorRoleName)]
        [HttpPost]
        public IActionResult Add(BestResultFormModel model)
        {
            if(model.BeforeFiles == null)
            {
                ModelState.AddModelError("BFiles", "The field is required!");
            }
            else
            {
                if(model.BeforeFiles.Count > 10)
                {
                    ModelState.AddModelError("BFiles", "The allowed limit for before photos is 10!");
                }
            }

            if(model.AfterFiles == null)
            {
                ModelState.AddModelError("AFiles", "The field is required!");
            }
            else
            {
                if (model.AfterFiles.Count > 10)
                {
                    ModelState.AddModelError("AFiles", "The allowed limit for after photos is 10!");
                }
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(x => x.Errors.Select(x => x.ErrorMessage)).ToList();
                return View(model);
            }

            bestResultService.AddBestResult(model);

            return RedirectToAction("All", "BestResults", new { area = "" });

        }

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Edit(int id)
        {
            var bestResult = bestResultService.CreateEditModel(id);

            if (bestResult == null)
            {
                return RedirectToAction("Error", "Home", new {area="", message = "This post is not available already" });
            }

            return View(bestResult);
        }

        [Authorize(Roles = AdministratorRoleName)]
        [HttpPost]
        public IActionResult Edit(int id, BestResultFormModel model, int rowVersion)
        {
            var br = bestResultService.GetBestResultById(id);
            if (br == null)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = "This post is not available already" });
            }
            var brBefore = br.Photos.Where(x => x.PhotoType == "Before").ToList().Count;
            var brAfter = br.Photos.Where(x => x.PhotoType == "After").ToList().Count;
            var startRowVersion = rowVersion;

            if (model.BeforeFiles != null)
            {
                if (brBefore +  model.BeforeFiles.Count > 10)
                {
                    ModelState.AddModelError("BFiles", $"You already have {brBefore} photos, you can only add {10 - brBefore} more!");
                }
            }

            if (model.AfterFiles != null)
            {
                if (brAfter + model.AfterFiles.Count > 10)
                {
                    ModelState.AddModelError("AFiles", $"You already have {brAfter} photos, you can only add {10 - brAfter} more!");
                }
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var isSuccess = bestResultService.EditBestResult(id, model, startRowVersion);
            if (isSuccess)
            {
                return RedirectToAction("Details", "BestResults", new { area = "", id = id });

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
            var bestResult = bestResultService.GetBestResultById(id);

            if (bestResult == null)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = "This post is not available already" });
            }

            if (!User.IsAdministrator())
            {
                return Unauthorized();
            }

            bestResultService.DeleteBestResult(bestResult);

            return RedirectToAction("All","BestResults", new { area = "" });
        }

        [Authorize(Roles = AdministratorRoleName)]
        [HttpPost]

        public bool RemovePhoto(int postId, int id)
        {
            return bestResultService.RemovePhoto(postId, id);
        }

        [Authorize(Roles = AdministratorRoleName)]
        [HttpPost]
        public bool RemoveAll(int postId)
        {
            return bestResultService.RemoveAllPhotos(postId);
        }

    }
}

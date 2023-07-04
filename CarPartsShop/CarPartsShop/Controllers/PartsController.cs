using CarPartsShop.Models.FormModels;
using CarPartsShop.Models.ViewModels;
using CarPartsShop.Services.PartService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata.Ecma335;
using static CarPartsShop.Infrastructure.RolesConstants;

namespace CarPartsShop.Controllers
{
    public class PartsController : Controller
    {
        public readonly IPartService partService;

        public PartsController(IPartService partService)
        {
            this.partService = partService;
        }

        [Authorize]
        public IActionResult Index([FromQuery] QueryModelParts query, string condition)
        {
            var parts = partService.GetAllParts(query,condition);
            return View(parts);
        }

        [Authorize]
        public IActionResult Details(string partId)
        {
            var part = partService.GetPartById(partId);

            if(part == null)
            {
                return BadRequest();
            }

            return View(part);
        }

        [Authorize(Roles = nameof(Administrator))]
        public IActionResult Add()
        {
            ViewData = partService.GetViewData(ViewData);
            return View();
        }

        [HttpPost]
        [Authorize(Roles = nameof(Administrator))]
        public IActionResult Add(PartsFormModel model)
        {
            partService.ValidatePart(model, ModelState);
            if (model.Files != null && model.Files.Count > 5)
            {
                ModelState.AddModelError("Files", "Позволеният лимит на снимките е 5");
            }
            if (!this.ModelState.IsValid)
            {
                ViewData = partService.GetViewData(ViewData);
                return View(model);
            }
            partService.CreatePart(model);
            return RedirectToAction("Index", "Parts");
        }

        [Authorize(Roles = nameof(Administrator))]
        public IActionResult Edit(string id)
        {
            var part = partService.GetPartById(id);

            if(part == null)
            {
                return BadRequest();
            }

            ViewData = partService.GetViewData(ViewData); 
            var formModel = partService.GetFormModel(part);

            return View(formModel);
        }

        [HttpPost]
        [Authorize(Roles = nameof(Administrator))]
        public IActionResult Edit(PartsFormModel model,string id)
        {
            var part = partService.GetPartById(id);

            if (part == null)
            {
                return BadRequest();
            }

            partService.ValidatePart(model, ModelState);
            if(model.Files != null)
            {
                if(part.PartImages.Count + model.Files.Count > 5)
                {
                    ModelState.AddModelError("Files", $"Позволеният лимит снимки е 5! Може да качите още {5 - part.PartImages.Count} снимки");
                }
            }

            if (!ModelState.IsValid)
            {
                ViewData = partService.GetViewData(ViewData);
                var formModel = partService.GetFormModel(part);

                return View(formModel);
            }

            partService.EditPart(model, part);

            return RedirectToAction("Index", "Parts");
        }

        [HttpGet]
        [Authorize(Roles = nameof(Administrator))]
        public IActionResult Delete(string id)
        {
            partService.DeletePart(id);

            return RedirectToAction("Index", "Parts");
        }
    }
}

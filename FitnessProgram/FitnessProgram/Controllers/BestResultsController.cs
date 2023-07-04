namespace FitnessProgram.Controllers
{
    using FitnessProgram.Infrastructure;
    using FitnessProgram.ViewModels.BestResult;
    using FitnessProgram.Services.BestResultService;
    using Microsoft.AspNetCore.Mvc;
    using FitnessProgram.Services.PostServices;
    using Microsoft.AspNetCore.Authorization;
    using static FitnessProgram.WebConstants;
    using FitnessProgram.Data.Models;

    public class BestResultsController : Controller
    {
        private readonly IBestResultService bestResultService;

        public BestResultsController(IBestResultService bestResultService)
        {
            this.bestResultService = bestResultService;
        }

        public IActionResult All([FromQuery] AllBestResultsQueryModel query)
        {
            var isAdministator = User.IsAdministrator();

            var currPageBestResults = bestResultService.GetAll(query.CurrentPage, AllBestResultsQueryModel.PostPerPage, query, isAdministator);

            return View(currPageBestResults);
        }

        public IActionResult Details(int id)
        {
            var model = bestResultService.GetDetails(id);

            if (model == null)
            {
                return RedirectToAction("Error", "Home", new { message = "This post is not available already" });
            }

            return View(model);
        }
    }
}

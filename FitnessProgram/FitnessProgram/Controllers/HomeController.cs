using FitnessProgram.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FitnessProgram.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string message)
        {
            return View(new ErrorViewModel {ErrorMessage = message});
        }
    }
}
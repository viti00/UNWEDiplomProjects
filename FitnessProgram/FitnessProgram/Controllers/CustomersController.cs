namespace FitnessProgram.Controllers
{
    using FitnessProgram.ViewModels.Customer;
    using FitnessProgram.Services.CustomerService;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using FitnessProgram.Infrastructure;
    using FitnessProgram.Data.Models;
    using System.Text.Json.Serialization;
    using System.Text.Json;

    public class CustomersController : Controller
    {
        private readonly ICustomerService customerService;

        public CustomersController(ICustomerService customerService)
            => this.customerService = customerService;

        [Authorize]
        public IActionResult MyReservations()
        {
            var reservations = customerService.GetMyReservations(User.GetId());

            return View(reservations);
        }

        [Authorize]
        public IActionResult TrainingReservation()
        {
            return View();
        }

        [Authorize]
        public string GetRanges(string id)
        {
            var dates = customerService.GetReservation(id);

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            var json = JsonSerializer.Serialize(dates, options);

            return json;
        }

        [Authorize]
        [HttpPost]
        public IActionResult TrainingReservation(ReservationFormModel model)
        {
            var userId = User.GetId();
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var isOkay = customerService.CreateReservation(model, userId);
            if (isOkay)
            {
                return Redirect("/Customers/MyReservations");
            }

            return BadRequest("Your reservation could not be completed, please try again");
        }

        
        [HttpPost]
        [Authorize]
        public bool RejectReservation(string id)
        {

            return customerService.RejectReservation(id);
        }

        [HttpPost]
        [Authorize]
        public bool EditReservation(string id, string name, string address, string phone)
        {

            return customerService.EditReservation(id,name,address,phone);
        }
    }
}

namespace FitnessProgram.Areas.Admin.Controllers
{
    using FitnessProgram.Data.Models;
    using FitnessProgram.Services.CustomerService;
    using FitnessProgram.ViewModels.Customer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using static WebConstants;

    [Area(AdminConstants.AreaName)]
    public class CustomersController : Controller
    {
        private readonly ICustomerService customerService;

        public CustomersController(ICustomerService customerService) 
            => this.customerService = customerService;

        [Authorize(Roles =AdministratorRoleName)]
        public IActionResult AllReservations()
        {
            return View();
        }

        [Authorize(Roles = AdministratorRoleName)]
        public List<AllReservationViewModel> GetAllReservations(string id)
        {
            var data = customerService.GetAllReservations(id);

            return data;
        }
        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Export()
        {
            var model = customerService.ExportToExcel();

            return File(model.Memory, model.Url, model.FileName);
        }

        public IActionResult Delete(string id)
        {
            var res = customerService.GetReservationById(id);
            if (res == null)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = "This reservation is not available already" });
            }
            customerService.RejectReservation(id);
            return Ok();
        }
    }
}

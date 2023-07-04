using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RestaurantOrders.Data;

namespace RestaurantOrders.Pages.Meals
{
    [Authorize(Roles ="Administrator")]
    public class CreateModel : PageModel
    {
        private readonly RestaurantOrders.Data.ApplicationDbContext _context;

        public CreateModel(RestaurantOrders.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Meal Meal { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            Meal.Id = Guid.NewGuid();
            Meal.LasLastModified_19118066 = DateTime.Now;
            Meal.IsInMenu = true;

            if (!ModelState.IsValid || _context.Meals == null || Meal == null)
            {

                ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name");
                return Page();
            }

            var log = new log_19118066
            {
                Id = Guid.NewGuid(),
                TableName = "Meals",
                CommandType = "Insert",
                ExecutionDate = DateTime.Now
            };

            await _context.log_19118066.AddAsync(log);

            await _context.Meals.AddAsync(Meal);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

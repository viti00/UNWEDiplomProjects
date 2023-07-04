using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantOrders.Data;

namespace RestaurantOrders.Pages.Meals
{
    [Authorize(Roles = "Administrator")]
    public class EditModel : PageModel
    {
        private readonly RestaurantOrders.Data.ApplicationDbContext _context;

        public EditModel(RestaurantOrders.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Meal Meal { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id, string operation)
        {
            if (id == null || _context.Meals == null)
            {
                return NotFound();
            }

            var meal =  await _context.Meals.FirstOrDefaultAsync(m => m.Id == id);
            if (meal == null)
            {
                return NotFound();
            }
            Meal = meal;

            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name");

            if(operation != null && operation == "remove")
            {
                await RemoveFromMenu(Meal);
                return RedirectToPage("./Index");
            }
            else if(operation != null && operation == "add")
            {
                await AddToMenu(Meal);
                return RedirectToPage("./Index");
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.

        public async Task RemoveFromMenu(Meal meal)
        {
            Meal.LasLastModified_19118066 = DateTime.Now;
            Meal.IsInMenu = false;

            var log = new log_19118066
            {
                Id = Guid.NewGuid(),
                TableName = "Meals",
                CommandType = "Update",
                ExecutionDate = DateTime.Now
            };

            await _context.log_19118066.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task AddToMenu(Meal meal)
        {
            Meal.LasLastModified_19118066 = DateTime.Now;
            Meal.IsInMenu = true;

            var log = new log_19118066
            {
                Id = Guid.NewGuid(),
                TableName = "Meals",
                CommandType = "Update",
                ExecutionDate = DateTime.Now
            };

            await _context.log_19118066.AddAsync(log);
            await _context.SaveChangesAsync();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            Meal.LasLastModified_19118066 = DateTime.Now;
            if (!ModelState.IsValid)
            {
                ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name");
                return Page();
            }

            _context.Attach(Meal).State = EntityState.Modified;

            var log = new log_19118066
            {
                Id = Guid.NewGuid(),
                TableName = "Meals",
                CommandType = "Update",
                ExecutionDate = DateTime.Now
            };

            await _context.log_19118066.AddAsync(log);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MealExists(Meal.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool MealExists(Guid id)
        {
          return (_context.Meals?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

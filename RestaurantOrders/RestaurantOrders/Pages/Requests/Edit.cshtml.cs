using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantOrders.Data;

namespace RestaurantOrders.Pages.Requests
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly RestaurantOrders.Data.ApplicationDbContext _context;

        public EditModel(RestaurantOrders.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Request Request { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id, string type)
        {
            if (id == null || _context.Requests == null)
            {
                return NotFound();
            }

            var request =  await _context.Requests.FirstOrDefaultAsync(m => m.Id == id);
            if (request == null)
            {
                return NotFound();
            }
            Request = request;

            if(type != null && type == "delete")
            {
                await Delete(request);
                return RedirectToPage("./Index");
            }

            return Page();
        }

        public async Task Delete(Request request)
        {
            _context.Requests.Remove(request);

            var log = new log_19118066
            {
                Id = Guid.NewGuid(),
                TableName = "Requests",
                CommandType = "Update",
                ExecutionDate = DateTime.Now
            };

            await _context.log_19118066.AddAsync(log);

            await _context.SaveChangesAsync();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            Request.LastModified_19118066 = DateTime.Now;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Request).State = EntityState.Modified;

            var log = new log_19118066
            {
                Id = Guid.NewGuid(),
                TableName = "Requests",
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
                if (!RequestExists(Request.Id))
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

        private bool RequestExists(Guid id)
        {
          return (_context.Requests?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RestaurantOrders.Data;

namespace RestaurantOrders.Pages.Requests
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly RestaurantOrders.Data.ApplicationDbContext _context;

        public CreateModel(RestaurantOrders.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Request Request { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            Request.Id = Guid.NewGuid();
            Request.RequestDate = DateTime.Now;
            Request.LastModified_19118066 = DateTime.Now;

            if (!ModelState.IsValid || _context.Requests == null || Request == null)
            {
                return Page();
            }

            var log = new log_19118066
            {
                Id = Guid.NewGuid(),
                TableName = "Requests",
                CommandType = "Insert",
                ExecutionDate = DateTime.Now
            };

            await _context.log_19118066.AddAsync(log);

            await _context.Requests.AddAsync(Request);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

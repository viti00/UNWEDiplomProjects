using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SteliTour.Data;

namespace SteliTour.Pages.Requests
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly SteliTour.Data.ApplicationDbContext _context;

        public CreateModel(SteliTour.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["UserId"] = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Page();
        }

        [BindProperty]
        public Request Request { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            Request.Id = Guid.NewGuid();
            Request.RequestDate = DateTime.Now;
            Request.LastModified_19118105 = DateTime.Now;
            Request.Status = "W";

            if (!ModelState.IsValid || _context.Request == null || Request == null)
            {
                ViewData["UserId"] = User.FindFirstValue(ClaimTypes.NameIdentifier);
                return Page();
            }

            var log = new log_19118105
            {
                Id = Guid.NewGuid(),
                Table = "Requests",
                Operation = "Insert",
                Date = DateTime.Now,
            };

            await _context.log_19118105.AddAsync(log);
            await _context.Request.AddAsync(Request);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index", new { type = "my" });
        }
    }
}

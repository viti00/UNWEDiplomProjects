using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SteliTour.Data;

namespace SteliTour.Pages.Destinations
{
    [Authorize(Roles = "Admin, Worker")]
    public class CreateModel : PageModel
    {
        private readonly SteliTour.Data.ApplicationDbContext _context;

        public CreateModel(SteliTour.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Destination Destination { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            Destination.Id = Guid.NewGuid();
            Destination.DateOfPublish = DateTime.Now;
            Destination.LastModified_19118105 = DateTime.Now;
            Destination.Status = "A";

            var log = new log_19118105
            {
                Id = Guid.NewGuid(),
                Table = "Destinations",
                Operation = "Insert",
                Date = DateTime.Now
            };

            if (!ModelState.IsValid || _context.Destination == null || Destination == null)
            {
                var errors = ModelState.Values.SelectMany(x => x.Errors.Select(c => c.ErrorMessage)).ToList();
                return Page();
            }

            await _context.Destination.AddAsync(Destination);
            await _context.log_19118105.AddAsync(log);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

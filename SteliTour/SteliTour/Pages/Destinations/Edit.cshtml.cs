using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SteliTour.Data;

namespace SteliTour.Pages.Destinations
{
    [Authorize(Roles = "Admin, Worker")]
    public class EditModel : PageModel
    {
        private readonly SteliTour.Data.ApplicationDbContext _context;

        public EditModel(SteliTour.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Destination Destination { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id, string handler)
        {
           
            if (id == null || _context.Destination == null)
            {
                return NotFound();
            }

            var destination =  await _context.Destination.FirstOrDefaultAsync(m => m.Id == id);
            if (destination == null)
            {
                return NotFound();
            }
            Destination = destination;

            if (handler != null && handler == "EditStatus")
            {
                Destination.Status = "C";
                var log = new log_19118105
                {
                    Id = Guid.NewGuid(),
                    Table = "Destinations",
                    Operation = "Update",
                    Date = DateTime.Now
                };
                await _context.AddAsync(log);
                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }
            else if (handler != null && handler == "Delete")
            {
                var log = new log_19118105
                {
                    Id = Guid.NewGuid(),
                    Table = "Destinations",
                    Operation = "Update",
                    Date = DateTime.Now
                };


                await _context.AddAsync(log);
                _context.Destination.Remove(destination);
                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Destination.LastModified_19118105 = DateTime.Now;

            _context.Attach(Destination).State = EntityState.Modified;

            var log = new log_19118105
            {
                Id = Guid.NewGuid(),
                Table = "Destinations",
                Operation = "Update",
                Date = DateTime.Now
            };
            await _context.AddAsync(log);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DestinationExists(Destination.Id))
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

        private bool DestinationExists(Guid id)
        {
          return (_context.Destination?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

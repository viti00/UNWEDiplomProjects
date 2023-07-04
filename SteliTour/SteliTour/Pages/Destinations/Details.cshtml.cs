using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SteliTour.Data;

namespace SteliTour.Pages.Destinations
{
    public class DetailsModel : PageModel
    {
        private readonly SteliTour.Data.ApplicationDbContext _context;

        public DetailsModel(SteliTour.Data.ApplicationDbContext context)
        {
            _context = context;
        }

      public Destination Destination { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.Destination == null)
            {
                return NotFound();
            }

            var destination = await _context.Destination.FirstOrDefaultAsync(m => m.Id == id);
            if (destination == null)
            {
                return NotFound();
            }
            else 
            {
                Destination = destination;
            }
            return Page();
        }
    }
}

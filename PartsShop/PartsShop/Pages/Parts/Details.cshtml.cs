using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PartsShop.Data;

namespace PartsShop.Pages.Parts
{
    public class DetailsModel : PageModel
    {
        private readonly PartsShop.Data.ApplicationDbContext _context;

        public DetailsModel(PartsShop.Data.ApplicationDbContext context)
        {
            _context = context;
        }

      public Part Part { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.Parts == null)
            {
                return NotFound();
            }

            var part = await _context
                .Parts
                .Include(x=> x.Category)
                .Include(x=> x.PartReviews)
                .ThenInclude(x=> x.User)
                .Include(x=> x.PartReviews)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (part == null)
            {
                return NotFound();
            }
            else 
            {
                Part = part;
            }
            return Page();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HomeEssentials.Data;

namespace HomeEssentials.Pages.Items
{
    public class DetailsModel : PageModel
    {
        private readonly HomeEssentials.Data.ApplicationDbContext _context;

        public DetailsModel(HomeEssentials.Data.ApplicationDbContext context)
        {
            _context = context;
        }

      public Item Item { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var item = await _context
                .Items
                .Include(x=> x.Reviews)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            else 
            {
                Item = item;
            }
            return Page();
        }
    }
}

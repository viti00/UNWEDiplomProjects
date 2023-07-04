using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HomeEssentials.Data;
using System.Security.Claims;

namespace HomeEssentials.Pages.Reviews
{
    public class IndexModel : PageModel
    {
        private readonly HomeEssentials.Data.ApplicationDbContext _context;

        public IndexModel(HomeEssentials.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Review> Review { get; set; } = default!;
        public Guid ItemId { get; set; }
        public async Task OnGetAsync(Guid id, string type)
        {

            if (_context.Reviews != null)
            {
                if (type != null && type == "My")
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    Review = await _context.Reviews
                    .Include(r => r.Item)
                    .Include(r => r.User)
                    .Where(x => x.UserId == userId)
                    .OrderByDescending(x => x.LastModified_19118075)
                    .ToListAsync();
                }
                else if (type != null && type == "All")
                {
                    Review = await _context.Reviews
                   .Include(r => r.Item)
                   .Include(r => r.User)
                   .OrderByDescending(x => x.LastModified_19118075)
                   .ToListAsync();
                }
                else
                {
                    Review = await _context.Reviews
                    .Include(r => r.Item)
                    .Include(r => r.User)
                    .Where(x => x.ItemId == id)
                    .OrderByDescending(x => x.LastModified_19118075)
                    .ToListAsync();

                    ItemId = id;
                }
            }
        }
    }
}

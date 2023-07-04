using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PartsShop.Data;

namespace PartsShop.Pages.PartReviews
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly PartsShop.Data.ApplicationDbContext _context;

        public IndexModel(PartsShop.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<PartReview> PartReview { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.PartReviews != null)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                PartReview = await _context.PartReviews
                .Include(p => p.User)
                .Where(x=> x.UserId == userId)
                .ToListAsync();
            }
        }
    }
}

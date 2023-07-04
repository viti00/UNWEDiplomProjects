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

namespace PartsShop.Pages.Orders
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly PartsShop.Data.ApplicationDbContext _context;

        public IndexModel(PartsShop.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Order> Order { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Orders != null)
            {
                if(User.IsInRole("Admin") || User.IsInRole("OrdersManager"))
                {
                    Order = await _context.Orders
                    .Include(o => o.User).ToListAsync();
                }
                else
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    Order = await _context.Orders
                   .Include(o => o.User)
                   .Where(x=> x.UserId == userId)
                   .ToListAsync();
                }
                
            }
        }
    }
}

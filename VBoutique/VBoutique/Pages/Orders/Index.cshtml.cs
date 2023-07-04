using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VBoutique.Data;

namespace VBoutique.Pages.Orders
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly VBoutique.Data.ApplicationDbContext _context;

        public IndexModel(VBoutique.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Order> Order { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if(User.IsInRole("OrdersManager") || User.IsInRole("Admin"))
            {
                if (_context.Orders != null)
                {
                    Order = await _context.Orders
                    .Include(o => o.User).ToListAsync();
                }
            }
            else
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (_context.Orders != null)
                {
                    Order = await _context.Orders
                    .Include(o => o.User)
                    .Where(x=> x.UserId == userId)
                    .ToListAsync();
                }
            }
           
        }
    }
}

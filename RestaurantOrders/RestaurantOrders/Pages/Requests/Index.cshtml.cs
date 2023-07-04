using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RestaurantOrders.Data;

namespace RestaurantOrders.Pages.Requests
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly RestaurantOrders.Data.ApplicationDbContext _context;

        public IndexModel(RestaurantOrders.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Request> Request { get;set; } = default!;

        public async Task OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (_context.Requests != null)
            {
                if (User.IsInRole("Administrator"))
                {
                    Request = await _context.Requests.Include(x=> x.User).ToListAsync();
                }
                else if(!User.IsInRole("DeliveryGuy") && !User.IsInRole("Administrator") && !User.IsInRole("Cooker"))
                {
                    Request = await _context.Requests.Include(x => x.User).Where(x=> x.UserId == userId).ToListAsync();
                }
               
            }
        }
    }
}

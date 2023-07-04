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

namespace RestaurantOrders.Pages.Orders
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly RestaurantOrders.Data.ApplicationDbContext _context;

        public IndexModel(RestaurantOrders.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Order> Order { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Orders != null)
            {
                if (User.IsInRole("Cooker"))
                {
                    Order = await _context.Orders
                    .Include(o => o.User).Where(x => x.Status == "Приготвя се").ToListAsync();
                }
                else if (User.IsInRole("DeliveryGuy"))
                {
                    Order = await _context.Orders
                    .Include(o => o.User).Where(x => x.Status == "Готова").ToListAsync();
                }
                else
                {
                    Order = await _context.Orders
                    .Include(o => o.User).Where(x => x.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier)).ToListAsync();
                }
                Order = Order.OrderBy(x => x.OrderDate).ToList();
            }
        }
    }
}

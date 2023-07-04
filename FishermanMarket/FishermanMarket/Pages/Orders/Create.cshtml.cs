using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FishermanMarket.Data;
using FishermanMarket.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace FishermanMarket.Pages.Orders
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly FishermanMarket.Data.ApplicationDbContext _context;

        public CreateModel(FishermanMarket.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Order Order { get; set; } = default!;
        public async Task<IActionResult> OnPostAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var bag = await _context.Bags
                .Include(x => x.Products)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.Image)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.UserId == userId && x.Status == "O");

            Order.Id = Guid.NewGuid();
            Order.Status = "Приета";
            Order.BagId = bag.Id;
            Order.UserId = userId;
            Order.User = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            Order.OrderDate = DateTime.Now;
            Order.LastModified_19118062 = DateTime.Now;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            bag.Status = "C";

            var order_log = new log_19118062
            {
                Id = Guid.NewGuid(),
                Table = "Orders",
                Operation = "Insert",
                Time = DateTime.Now
            };
            var bag_log = new log_19118062
            {
                Id = Guid.NewGuid(),
                Table = "Bags",
                Operation = "Update",
                Time = DateTime.Now
            };

            _context.log_19118062.Add(order_log);
            _context.log_19118062.Add(bag_log);
            _context.Orders.Add(Order);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

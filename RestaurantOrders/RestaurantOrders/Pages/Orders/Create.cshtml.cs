using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantOrders.Data;

namespace RestaurantOrders.Pages.Orders
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly RestaurantOrders.Data.ApplicationDbContext _context;

        public CreateModel(RestaurantOrders.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var bagId = _context.Bags.Include(x => x.User).FirstOrDefault(x => x.UserId == userId && x.Status == "A").Id;

            if(bagId == null)
            {
                return NotFound();
            }

            ViewData["UserId"] = userId;
            ViewData["BagId"] = bagId;
            return Page();
        }

        [BindProperty]
        public Order Order { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Order.Id = Guid.NewGuid();
            Order.OrderDate = DateTime.Now;
            Order.LastModified_19118066 = DateTime.Now;
            Order.Status = "Приготвя се";

            if (!ModelState.IsValid || _context.Orders == null || Order == null)
            {
                
                ViewData["UserId"] = userId;
                ViewData["BagId"] = _context.Bags.Include(x => x.User).FirstOrDefault(x => x.UserId == userId && x.Status == "A").Id;
                return Page();
            }

            await _context.Orders.AddAsync(Order);

            var log_order = new log_19118066
            {
                Id = Guid.NewGuid(),
                TableName = "Orders",
                CommandType = "Insert",
                ExecutionDate = DateTime.Now,
            };

            await _context.log_19118066.AddAsync(log_order);

            var bag = await _context.Bags.Include(x => x.User).FirstOrDefaultAsync(x => x.UserId == userId && x.Status == "A");
            bag.Status = "C";
            bag.LastModified_19118066 = DateTime.Now;

            var log_bag = new log_19118066
            {
                Id = Guid.NewGuid(),
                TableName = "Bags",
                CommandType = "Update",
                ExecutionDate = DateTime.Now,
            };

            await _context.log_19118066.AddAsync(log_bag);

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

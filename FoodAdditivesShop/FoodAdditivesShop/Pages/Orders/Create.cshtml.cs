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
using FoodAdditivesShop.Data;

namespace FoodAdditivesShop.Pages.Orders
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly FoodAdditivesShop.Data.ApplicationDbContext _context;

        public CreateModel(FoodAdditivesShop.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["UserId"] = userId;
            ViewData["CartId"] = _context.Carts.Include(x => x.User).FirstOrDefault(x => x.UserId == userId && x.IsOrdered == false).Id;
            return Page();
        }

        [BindProperty]
        public Order Order { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            Order.Id = Guid.NewGuid();
            Order.LastModified_19118119 = DateTime.Now;
            Order.Status = "Приета";

            if (!ModelState.IsValid || _context.Orders == null || Order == null)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                ViewData["UserId"] = userId;
                ViewData["CartId"] =  _context.Carts.Include(x=> x.User).FirstOrDefault(x => x.UserId == userId && x.IsOrdered == false).Id;
                return Page();
            }

            

            await _context.Orders.AddAsync(Order);

            var log_order = new log_19118119
            {
                Id = Guid.NewGuid(),
                Table = "Orders",
                Action = "Insert",
                ActionTime = DateTime.Now
            };

            var cart = await _context.Carts.FirstOrDefaultAsync(x => x.Id == Order.CartId);
            cart.IsOrdered = true;
            cart.LastModified_19118119 = DateTime.Now;

            var log_cart = new log_19118119
            {
                Id = Guid.NewGuid(),
                Table = "Carts",
                Action = "Update",
                ActionTime = DateTime.Now
            };

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

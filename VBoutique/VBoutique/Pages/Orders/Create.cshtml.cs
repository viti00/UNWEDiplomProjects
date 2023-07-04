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
using VBoutique.Data;

namespace VBoutique.Pages.Orders
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly VBoutique.Data.ApplicationDbContext _context;

        public CreateModel(VBoutique.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["UserId"] = userId;
            ViewData["CartId"] = _context
                .ShoppingCarts
                .Include(x=> x.User)
                .FirstOrDefault(x => x.UserId == userId && x.IsActive == true).Id;
            return Page();
        }

        [BindProperty]
        public Order Order { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartId = _context
                    .ShoppingCarts
                    .Include(x => x.User)
                    .FirstOrDefault(x => x.UserId == userId && x.IsActive == true).Id;

            Order.Id = Guid.NewGuid();
            Order.LastModified_19118155 = DateTime.Now;

            if (!ModelState.IsValid || _context.Orders == null || Order == null)
            {
                
                ViewData["UserId"] = userId;
                ViewData["CartId"] = cartId;
                return Page();
            }

            var cart = await _context.ShoppingCarts.FirstOrDefaultAsync(x=> x.Id == cartId);
            cart.IsActive = false;
            cart.LastModified_19118155 = DateTime.Now;

            await _context.Orders.AddAsync(Order);

            var logOrder = new log_19118155
            {
                Id = Guid.NewGuid(),
                TableName = "Orders",
                OperationType = "Insert",
                OperationTime = DateTime.Now
            };

            await _context.log_19118155.AddAsync(logOrder);

            var logCart = new log_19118155
            {
                Id = Guid.NewGuid(),
                TableName = "ShoppingCarts",
                OperationType = "Update",
                OperationTime = DateTime.Now
            };

            await _context.log_19118155.AddAsync(logCart);

            await _context.SaveChangesAsync();

            return RedirectToPage("/Orders/Index");
        }
    }
}

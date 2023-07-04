using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using HomeEssentials.Data;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace HomeEssentials.Pages.Orders
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly HomeEssentials.Data.ApplicationDbContext _context;

        public CreateModel(HomeEssentials.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["UserId"] = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["CartId"] = _context.Carts.FirstOrDefault(x => x.UserId == ViewData["UserId"] && x.Status.Name == "Active").Id;
            return Page();
        }

        [BindProperty]
        public Order Order { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            Order.Id = Guid.NewGuid();
            Order.CreatedOn = DateTime.Now;
            Order.LastModified_19118075 = DateTime.Now;
            Order.Status = await _context.OrderStatuses.FirstOrDefaultAsync(x => x.Name == "Awaiting");

            if (!ModelState.IsValid || _context.Orders == null || Order == null)
            {
                ViewData["UserId"] = User.FindFirstValue(ClaimTypes.NameIdentifier);
                ViewData["CartId"] = _context.Carts.FirstOrDefault(x => x.UserId == ViewData["UserId"] && x.Status.Name == "Active").Id;
                return Page();
            }

            var cart = await  _context.Carts.FirstOrDefaultAsync(x => x.Id == Order.CartId && x.Status.Name == "Active");

            cart.Status = await _context.CartStatuses.FirstOrDefaultAsync(x => x.Name == "Ordered");

            var logCart = new log_19118075
            {
                Id = Guid.NewGuid(),
                Table = "Carts",
                Command = "Update",
                DateTime = DateTime.Now
            };

            await _context.log_19118075.AddAsync(logCart);

            var log = new log_19118075
            {
                Id = Guid.NewGuid(),
                Table = "Orders",
                Command = "Insert",
                DateTime = DateTime.Now
            };

            await _context.log_19118075.AddAsync(log);

            await _context.Orders.AddAsync(Order);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

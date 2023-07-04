using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FishermanMarket.Data;
using FishermanMarket.Data.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FishermanMarket.Pages.Orders
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly FishermanMarket.Data.ApplicationDbContext _context;

        public EditModel(FishermanMarket.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Order Order { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order =  await _context.Orders.FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            Order = order;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["UserId"] = _context.Users.FirstOrDefault(x => x.Id == userId).Id;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Order.LastModified_19118062 = DateTime.Now;

            var order_log = new log_19118062
            {
                Id = Guid.NewGuid(),
                Table = "Orders",
                Operation = "Update",
                Time = DateTime.Now
            };

            _context.log_19118062.Add(order_log);

            _context.Attach(Order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(Order.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool OrderExists(Guid id)
        {
          return (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

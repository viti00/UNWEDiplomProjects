using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HomeEssentials.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace HomeEssentials.Pages.Orders
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly HomeEssentials.Data.ApplicationDbContext _context;

        public EditModel(HomeEssentials.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Order Order { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id, string status)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            if(status != null)
            {
                await ChangeStatus(order, status);
                return RedirectToPage("./Index");
            }
            Order = order;
            ViewData["UserId"] = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Page();
        }

        public async Task ChangeStatus(Order order, string status)
        {
            order.Status = await _context.OrderStatuses.FirstOrDefaultAsync(x => x.Name == status);

            var log = new log_19118075
            {
                Id = Guid.NewGuid(),
                Table = "Orders",
                Command = "Update",
                DateTime = DateTime.Now
            };

            await _context.log_19118075.AddAsync(log);

            await _context.SaveChangesAsync();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            Order.LastModified_19118075 = DateTime.Now;

            if (!ModelState.IsValid)
            {
                ViewData["UserId"] = User.FindFirstValue(ClaimTypes.NameIdentifier);
                return Page();
            }

            _context.Attach(Order).State = EntityState.Modified;

            var log = new log_19118075
            {
                Id = Guid.NewGuid(),
                Table = "Orders",
                Command = "Insert",
                DateTime = DateTime.Now
            };

            await _context.log_19118075.AddAsync(log);

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

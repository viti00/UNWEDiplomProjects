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
    public class EditModel : PageModel
    {
        private readonly VBoutique.Data.ApplicationDbContext _context;

        public EditModel(VBoutique.Data.ApplicationDbContext context)
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
            ViewData["UserId"] = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["UserId"] = User.FindFirstValue(ClaimTypes.NameIdentifier);
                return Page();
            }

            Order.LastModified_19118155 = DateTime.Now;

            var log = new log_19118155
            {
                Id = Guid.NewGuid(),
                TableName = "Orders",
                OperationType = "Update",
                OperationTime = DateTime.Now
            };

            await _context.log_19118155.AddAsync(log);

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

            return RedirectToPage("/Orders/Index");
        }

        public async Task<IActionResult> OnGetReject(Guid id)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == id);

            _context.Orders.Remove(order);

            var log = new log_19118155
            {
                Id = Guid.NewGuid(),
                TableName = "Orders",
                OperationType = "Update",
                OperationTime = DateTime.Now
            };

            await _context.log_19118155.AddAsync(log);

            await _context.SaveChangesAsync();

            return RedirectToPage("/Orders/Index");
        }

        private bool OrderExists(Guid id)
        {
          return (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

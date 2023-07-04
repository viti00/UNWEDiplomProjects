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
using PartsShop.Data;

namespace PartsShop.Pages.Orders
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly PartsShop.Data.ApplicationDbContext _context;

        public EditModel(PartsShop.Data.ApplicationDbContext context)
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            Order.LastModified_19118073 = DateTime.Now;
            if (!ModelState.IsValid)
            {
                ViewData["UserId"] = User.FindFirstValue(ClaimTypes.NameIdentifier);
                return Page();
            }

            _context.Attach(Order).State = EntityState.Modified;


            var log_order = new log_19118073
            {
                Id = Guid.NewGuid(),
                Table = "Orders",
                Action = "Update",
                ActionTime = DateTime.Now
            };

            await _context.log_19118073.AddAsync(log_order);

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

        public async Task<IActionResult> OnGetDelete(Guid id)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == id);

            if (order != null)
            {
                _context.Orders.Remove(order);

                var log_order = new log_19118073
                {
                    Id = Guid.NewGuid(),
                    Table = "Orders",
                    Action = "Update",
                    ActionTime = DateTime.Now
                };

                await _context.log_19118073.AddAsync(log_order);

                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

        private bool OrderExists(Guid id)
        {
          return (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

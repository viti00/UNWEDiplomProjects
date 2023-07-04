using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
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
    public class EditModel : PageModel
    {
        private readonly RestaurantOrders.Data.ApplicationDbContext _context;

        public EditModel(RestaurantOrders.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Order Order { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id, string type)
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

            if(type != null && type == "reject")
            {
                await Reject(Order);
                return RedirectToPage("/Orders/Index");
            }
            else if(type != null && type == "ready")
            {
                await Ready(Order);
                return RedirectToPage("/Orders/Index");
            }
            else if(type != null && type == "delivered")
            {
                await Delivered(Order);
                return RedirectToPage("/Orders/Index");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["UserId"] = userId;
            ViewData["BagId"] = Order.BagId;
            return Page();
        }

        public async Task Reject(Order order)
        {
            var bag = await _context.Bags
                 .Include(x => x.User)
                .Include(x => x.Meals)
                .ThenInclude(x => x.Meal)
                .FirstOrDefaultAsync(x => x.Id == order.BagId);

            foreach (var item in bag.Meals)
            {

                bag.Meals.Remove(item);

                var log_sp = new log_19118066
                {
                    Id = Guid.NewGuid(),
                    TableName = "SelectedProduct",
                    CommandType = "Update",
                    ExecutionDate = DateTime.Now,
                };

                await _context.log_19118066.AddAsync(log_sp);
            }

            _context.Orders.Remove(order);

            var log_order = new log_19118066
            {
                Id = Guid.NewGuid(),
                TableName = "Orders",
                CommandType = "Update",
                ExecutionDate = DateTime.Now,
            };

            await _context.log_19118066.AddAsync(log_order);

            await _context.SaveChangesAsync();
        }

        public async Task Ready(Order order)
        {
            order.Status = "Готова";
            order.LastModified_19118066 = DateTime.Now;

            var log_order = new log_19118066
            {
                Id = Guid.NewGuid(),
                TableName = "Orders",
                CommandType = "Update",
                ExecutionDate = DateTime.Now,
            };

            await _context.log_19118066.AddAsync(log_order);

            await _context.SaveChangesAsync();
        }

        public async Task Delivered(Order order)
        {
            order.Status = "Доставена";
            order.LastModified_19118066 = DateTime.Now;

            var log_order = new log_19118066
            {
                Id = Guid.NewGuid(),
                TableName = "Orders",
                CommandType = "Update",
                ExecutionDate = DateTime.Now,
            };

            await _context.log_19118066.AddAsync(log_order);

            await _context.SaveChangesAsync();
        }
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            Order.LastModified_19118066 = DateTime.Now;


            if (!ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                ViewData["UserId"] = userId;
                ViewData["BagId"] = Order.BagId;
                return Page();
            }

            _context.Attach(Order).State = EntityState.Modified;

            var log_order = new log_19118066
            {
                Id = Guid.NewGuid(),
                TableName = "Orders",
                CommandType = "Insert",
                ExecutionDate = DateTime.Now,
            };

            await _context.log_19118066.AddAsync(log_order);

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

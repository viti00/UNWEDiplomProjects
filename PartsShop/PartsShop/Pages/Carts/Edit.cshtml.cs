using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PartsShop.Data;

namespace PartsShop.Pages.Carts
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
        public CartPart CartPart { get; set; } = default!;

        public async Task<IActionResult> OnGetRemoveAll(Guid cartId)
        {
            var parts = await _context.CartParts.Include(x => x.Cart).Where(x => x.CartId == cartId).ToListAsync();

            if (parts.Count > 0)
            {
                foreach (var part in parts)
                {
                    _context.CartParts.Remove(part);

                    var log = new log_19118073
                    {
                        Id = Guid.NewGuid(),
                        Table = "CartParts",
                        Action = "Update",
                        ActionTime = DateTime.Now,
                    };

                    await _context.log_19118073.AddAsync(log);

                }

                var log_cart = new log_19118073
                {
                    Id = Guid.NewGuid(),
                    Table = "Carts",
                    Action = "Update",
                    ActionTime = DateTime.Now,
                };
                await _context.log_19118073.AddAsync(log_cart);

                await _context.SaveChangesAsync();
            }


            return RedirectToPage("/Carts/Details");
        }

        public async Task<IActionResult> OnGetRemovePart(Guid partId, Guid cartId)
        {

            var part = await _context
                .CartParts
                .Include(x => x.Part)
                .Include(x => x.Cart)
                .FirstOrDefaultAsync(x => x.PartId == partId && x.CartId == cartId);

            if(part != null)
            {
                _context.CartParts.Remove(part);

                var log = new log_19118073
                {
                    Id = Guid.NewGuid(),
                    Table = "CartParts",
                    Action = "Update",
                    ActionTime = DateTime.Now,
                };

                await _context.log_19118073.AddAsync(log);

                var log_cart = new log_19118073
                {
                    Id = Guid.NewGuid(),
                    Table = "Carts",
                    Action = "Update",
                    ActionTime = DateTime.Now,
                };
                await _context.log_19118073.AddAsync(log_cart);

                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Carts/Details");
        }
    }
}

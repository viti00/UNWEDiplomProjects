using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodAdditivesShop.Data;

namespace FoodAdditivesShop.Pages.Carts
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly FoodAdditivesShop.Data.ApplicationDbContext _context;

        public EditModel(FoodAdditivesShop.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CartProduct CartProducts { get; set; } = default!;

        public async Task<IActionResult> OnGetRemoveAll(Guid cartId)
        {
            var products = await _context.CartProducts.Include(x => x.Cart).Where(x => x.CartId == cartId).ToListAsync();

            if (products.Count > 0)
            {
                foreach (var product in products)
                {
                    _context.CartProducts.Remove(product);

                    var log = new log_19118119
                    {
                        Id = Guid.NewGuid(),
                        Table = "CartProducts",
                        Action = "Update",
                        ActionTime = DateTime.Now,
                    };

                    await _context.log_19118119.AddAsync(log);

                }

                var log_cart = new log_19118119
                {
                    Id = Guid.NewGuid(),
                    Table = "Carts",
                    Action = "Update",
                    ActionTime = DateTime.Now,
                };
                await _context.log_19118119.AddAsync(log_cart);

                await _context.SaveChangesAsync();
            }


            return RedirectToPage("/Carts/Details");
        }

        public async Task<IActionResult> OnGetRemovePart(Guid productId, Guid cartId)
        {

            var product = await _context
                .CartProducts
                .Include(x => x.Product)
                .Include(x => x.Cart)
                .FirstOrDefaultAsync(x => x.ProductId == productId && x.CartId == cartId);

            if(product != null)
            {
                _context.CartProducts.Remove(product);

                var log = new log_19118119
                {
                    Id = Guid.NewGuid(),
                    Table = "CartProducts",
                    Action = "Update",
                    ActionTime = DateTime.Now,
                };

                await _context.log_19118119.AddAsync(log);

                var log_cart = new log_19118119
                {
                    Id = Guid.NewGuid(),
                    Table = "Carts",
                    Action = "Update",
                    ActionTime = DateTime.Now,
                };
                await _context.log_19118119.AddAsync(log_cart);

                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Carts/Details");
        }
    }
}

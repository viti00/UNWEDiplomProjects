using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VBoutique.Data;

namespace VBoutique.Pages.ShoppingCarts
{
    [Authorize]
    public class InfoModel : PageModel
    {
        private readonly VBoutique.Data.ApplicationDbContext _context;

        public InfoModel(VBoutique.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public ShoppingCart ShoppingCart { get; set; } = default!;
        public double TotalPrice { get; set; }

        public async Task<IActionResult> OnGetAsync(string type, Guid cartId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ShoppingCart shoppingcart = null;
            if(type != null && type == "Closed")
            {
                shoppingcart = await _context
                .ShoppingCarts
                .Include(x => x.User)
                .Include(x => x.ShoppingCartItems)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x=> x.Id == cartId);
            }
            else
            {
                shoppingcart = await _context
                .ShoppingCarts
                .Include(x=> x.User)
                .Include(x => x.ShoppingCartItems)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.UserId == userId && x.IsActive == true);
            }

            ShoppingCart = shoppingcart;
            if(ShoppingCart != null)
            {
                TotalPrice = CalculateTotalPrice(shoppingcart.ShoppingCartItems);
            }
            return Page();
        }

        private double CalculateTotalPrice(List<ShoppingCartItems> items)
        {
            double total = 0;

            foreach (var item in items)
            {
                if(item.Product is Bag)
                {
                    total += item.Product.Price;
                }
                else if(item.Product is Shoe)
                {
                    total += (item.Size35Count + item.Size36Count + item.Size37Count
                          + item.Size38Count + item.Size39Count + item.Size40Count + item.Size41Count) * item.Product.Price;
                };
            }

            return total;
        }


        public async Task<IActionResult> OnGetProductsCount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var shoppingcart = await _context
                .ShoppingCarts
                .Include(x => x.User)
                .Include(x => x.ShoppingCartItems)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.UserId == userId && x.IsActive == true);

            return new JsonResult(shoppingcart != null? shoppingcart.ShoppingCartItems.Count: 0);
        }
    }
}

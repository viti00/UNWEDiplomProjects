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
using FoodAdditivesShop.Data;
using Microsoft.CodeAnalysis;

namespace FoodAdditivesShop.Pages.Carts
{
    [Authorize]
    public class AddToCartModel : PageModel
    {
        private readonly FoodAdditivesShop.Data.ApplicationDbContext _context;

        public AddToCartModel(FoodAdditivesShop.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(Guid productId, int count)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _context.Carts
                .Include(x=> x.CartProducts)
                .ThenInclude(x=> x.Product)
                .FirstOrDefaultAsync(x=> x.UserId == userId && x.IsOrdered == false);

            if (cart == null)
            {
                cart = await CreateNewCart(userId);
            }

            if(cart.CartProducts.Any(x=> x.ProductId == productId))
            {
                var product = cart.CartProducts.FirstOrDefault(x=> x.ProductId == productId);

                product.ProductCount += count;

                var log = new log_19118119
                {
                    Id = Guid.NewGuid(),
                    Table = "CartProducts",
                    Action = "Update",
                    ActionTime = DateTime.Now,
                };

                await _context.log_19118119.AddAsync(log);
            }
            else
            {
                var cartProduct = CreateCartProduct(productId, cart.Id, count);

                cart.CartProducts.Add(cartProduct);

                var log = new log_19118119
                {
                    Id = Guid.NewGuid(),
                    Table = "CartProducts",
                    Action = "Insert",
                    ActionTime = DateTime.Now,
                };

                await _context.log_19118119.AddAsync(log);
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("/Products/Details", new { id = productId });
        }

        private CartProduct CreateCartProduct(Guid productId, Guid cartId, int count)
        {
            var cartProduct = new CartProduct
            {
                Id = Guid.NewGuid(),
                CartId = cartId,
                ProductId = productId,
                ProductCount = count,
                LastModified_19118119 = DateTime.Now,
            };

            return cartProduct;
        }

        private async Task<Cart> CreateNewCart(string userId)
        {
            var cart = new Cart
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CartProducts = new List<CartProduct>(),
                IsOrdered = false,
                LastModified_19118119 = DateTime.Now
            };

            var log = new log_19118119
            {
                Id = Guid.NewGuid(),
                Table = "Carts",
                Action = "Insert",
                ActionTime = DateTime.Now,
            };

            await _context.Carts.AddAsync(cart);
            await _context.log_19118119.AddAsync(log);
            await _context.SaveChangesAsync();

            return cart;
        }
        
    }
}

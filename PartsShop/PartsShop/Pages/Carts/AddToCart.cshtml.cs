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

namespace PartsShop.Pages.Carts
{
    [Authorize]
    public class AddToCartModel : PageModel
    {
        private readonly PartsShop.Data.ApplicationDbContext _context;

        public AddToCartModel(PartsShop.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(Guid partId, int count)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _context.Carts
                .Include(x=> x.CartParts)
                .ThenInclude(x=> x.Part)
                .FirstOrDefaultAsync(x=> x.UserId == userId && x.IsOrdered == false);

            if (cart == null)
            {
                cart = await CreateNewCart(userId);
            }

            if(cart.CartParts.Any(x=> x.PartId == partId))
            {
                var part = cart.CartParts.FirstOrDefault(x=> x.PartId == partId);

                part.PartCount += count;

                var log = new log_19118073
                {
                    Id = Guid.NewGuid(),
                    Table = "CartParts",
                    Action = "Update",
                    ActionTime = DateTime.Now,
                };

                await _context.log_19118073.AddAsync(log);
            }
            else
            {
                var cartPart = CreateCartPart(partId, cart.Id, count);

                cart.CartParts.Add(cartPart);

                var log = new log_19118073
                {
                    Id = Guid.NewGuid(),
                    Table = "CartParts",
                    Action = "Insert",
                    ActionTime = DateTime.Now,
                };

                await _context.log_19118073.AddAsync(log);
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("/Parts/Details", new { id = partId });
        }

        private CartPart CreateCartPart(Guid partId, Guid cartId, int count)
        {
            var cartPart = new CartPart
            {
                Id = Guid.NewGuid(),
                CartId = cartId,
                PartId = partId,
                PartCount = count,
                LastModified_19118073 = DateTime.Now,
            };

            return cartPart;
        }

        private async Task<Cart> CreateNewCart(string userId)
        {
            var cart = new Cart
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CartParts = new List<CartPart>(),
                IsOrdered = false,
                LastModified_19118073 = DateTime.Now
            };

            var log = new log_19118073
            {
                Id = Guid.NewGuid(),
                Table = "Carts",
                Action = "Insert",
                ActionTime = DateTime.Now,
            };

            await _context.Carts.AddAsync(cart);
            await _context.log_19118073.AddAsync(log);
            await _context.SaveChangesAsync();

            return cart;
        }
        
    }
}

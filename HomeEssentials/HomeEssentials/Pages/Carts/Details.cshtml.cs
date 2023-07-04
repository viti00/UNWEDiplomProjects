using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HomeEssentials.Data;
using System.Security.Claims;
using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace HomeEssentials.Pages.Carts
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly HomeEssentials.Data.ApplicationDbContext _context;

        public DetailsModel(HomeEssentials.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Cart Cart { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid id, string type)
        {
            Cart cart = null;

            if (type != null && type == "Ordered")
            {
                cart = await _context.Carts
                    .Include(x => x.User)
                    .Include(x => x.CartItems)
                    .ThenInclude(x => x.Item)
                    .Include(x => x.Status)
                    .FirstOrDefaultAsync(x => x.Id == id);
            }
            else
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                cart = await _context.Carts
                 .Include(x => x.User)
                 .Include(x => x.CartItems)
                 .ThenInclude(x => x.Item)
                 .Include(x => x.Status)
                 .FirstOrDefaultAsync(x => x.Status.Name == "Active" && x.UserId == userId);
            }


            Cart = cart;

            return Page();
        }

        public async Task<IActionResult> OnPostAddToCart(int count, Guid itemId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _context.Carts
               .Include(x => x.User)
               .Include(x => x.Status)
               .Include(x => x.CartItems)
               .ThenInclude(x => x.Item)
               .FirstOrDefaultAsync(x => x.Status.Name == "Active" && x.UserId == userId);

            if (cart == null)
            {
                cart = await CreateCart(userId);
            }

            var item = await _context.Items.FirstOrDefaultAsync(x => x.Id == itemId);

            if (item != null)
            {
                await AddItem(item, cart, count);
            }

            return RedirectToPage("/Items/Index");
        }

        public async Task<IActionResult> OnGetCartCount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _context.Carts
               .Include(x => x.User)
               .Include(x => x.Status)
               .Include(x => x.CartItems)
               .ThenInclude(x => x.Item)
               .FirstOrDefaultAsync(x => x.Status.Name == "Active" && x.UserId == userId);
            var count = 0;
            if (cart != null)
            {
                count = cart.CartItems.Count;
            }

            return new JsonResult(count);
        }
        private async Task<Cart> CreateCart(string userId)
        {

            var cart = new Cart
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CartItems = new List<ItemInCart>(),
                CartStatusId = _context.CartStatuses.FirstOrDefault(x => x.Name == "Active").Id,
                LastModified_19118075 = DateTime.Now
            };

            await _context.Carts.AddAsync(cart);
            var log = new log_19118075
            {
                Id = Guid.NewGuid(),
                Table = "Carts",
                Command = "Insert",
                DateTime = DateTime.Now
            };

            await _context.log_19118075.AddAsync(log);
            await _context.SaveChangesAsync();

            return cart;
        }



        private async Task AddItem(Item item, Cart cart, int count)
        {
            var cartItem = cart.CartItems.FirstOrDefault(x => x.ItemId == item.Id);
            if (cartItem != null)
            {
                cartItem.ItemCount += count;
                var log = new log_19118075
                {
                    Id = Guid.NewGuid(),
                    Table = "ItemsInCart",
                    Command = "Update",
                    DateTime = DateTime.Now
                };

                await _context.log_19118075.AddAsync(log);
            }
            else
            {
                cartItem = new ItemInCart()
                {
                    Id = Guid.NewGuid(),
                    ItemId = item.Id,
                    CartId = cart.Id,
                    ItemCount = count,
                    LastModified_19118075 = DateTime.Now
                };
                await _context.ItemsInCart.AddAsync(cartItem);
                var log = new log_19118075
                {
                    Id = Guid.NewGuid(),
                    Table = "ItemsInCart",
                    Command = "Insert",
                    DateTime = DateTime.Now
                };

                await _context.log_19118075.AddAsync(log);
            }

            await _context.SaveChangesAsync();


        }

        public async Task<IActionResult> OnGetChangeCount(Guid itemId, int count)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _context.Carts
               .Include(x => x.User)
               .Include(x => x.Status)
               .Include(x => x.CartItems)
               .ThenInclude(x => x.Item)
               .FirstOrDefaultAsync(x => x.Status.Name == "Active" && x.UserId == userId);

            if (cart != null)
            {
                var itemInCart = cart.CartItems.FirstOrDefault(x => x.ItemId == itemId);

                if (itemInCart != null)
                {
                    itemInCart.ItemCount = count;
                    var log = new log_19118075
                    {
                        Id = Guid.NewGuid(),
                        Table = "ItemsInCart",
                        Command = "Update",
                        DateTime = DateTime.Now
                    };

                    await _context.log_19118075.AddAsync(log);
                    await _context.SaveChangesAsync();
                }
            }

            return new JsonResult(cart.CartItems.Sum(x => x.ItemCount * x.Item.Price));
        }

        public async Task OnGetRemoveFromCart(Guid itemId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _context.Carts
               .Include(x => x.User)
               .Include(x => x.Status)
               .Include(x => x.CartItems)
               .ThenInclude(x => x.Item)
               .FirstOrDefaultAsync(x => x.Status.Name == "Active" && x.UserId == userId);

            if (cart != null)
            {
                var itemInCart = cart.CartItems.FirstOrDefault(x => x.ItemId == itemId);

                if (itemInCart != null)
                {
                    cart.CartItems.Remove(itemInCart);

                    var log = new log_19118075
                    {
                        Id = Guid.NewGuid(),
                        Table = "ItemsInCart",
                        Command = "Update",
                        DateTime = DateTime.Now
                    };

                    await _context.log_19118075.AddAsync(log);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }


}

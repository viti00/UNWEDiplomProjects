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

namespace VBoutique.Pages.ShoppingCarts
{
    [Authorize]
    public class AddToCartModel : PageModel
    {
        private readonly VBoutique.Data.ApplicationDbContext _context;

        public AddToCartModel(VBoutique.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ShoppingCartItems ShoppingCartItems { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync(Guid productId, int count, string size)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var shoppingcart = await _context
                 .ShoppingCarts
                 .Include(x => x.ShoppingCartItems)
                 .ThenInclude(x => x.Product)
                 .FirstOrDefaultAsync(x => x.UserId == userId && x.IsActive == true);



            if (shoppingcart == null)
            {
                shoppingcart = await CreateCart(userId);
            }

            var product = await _context.Product.FirstOrDefaultAsync(x => x.Id == productId);

            ShoppingCartItems cartItem = null;

            if (product != null)
            {
                cartItem = shoppingcart.ShoppingCartItems.FirstOrDefault(x => x.ProductId == product.Id);
                if (cartItem == null)
                {
                    cartItem = CreateCartItem(product, count, size, shoppingcart.Id);

                    await _context.ShoppingCartItems.AddAsync(cartItem);

                    var log = new log_19118155
                    {
                        Id = Guid.NewGuid(),
                        TableName = "ShoppingCartItems",
                        OperationType = "Insert",
                        OperationTime = DateTime.Now
                    };

                    await _context.log_19118155.AddAsync(log);
                }
                else
                {
                    if(product is Bag)
                    {
                        cartItem.ProductCount += count;
                    }
                    else if (product is Shoe)
                    {
                       cartItem = AddSizes(size, cartItem, count);
                    }
                    

                    var log = new log_19118155
                    {
                        Id = Guid.NewGuid(),
                        TableName = "ShoppingCartItems",
                        OperationType = "Update",
                        OperationTime = DateTime.Now
                    };

                    await _context.log_19118155.AddAsync(log);
                }
                
            }
            else
            {
                return NotFound();
            }

            await _context.SaveChangesAsync();
            string type = "";
            if(product is Bag)
            {
                type = "Bags";
            }
            else if(product is Shoe)
            {
                type = "Shoes";
            }

            return RedirectToPage("/Products/Details", new {id=product.Id,type=type});
        }

        private ShoppingCartItems CreateCartItem(Product product, int count, string size, Guid cartId)
        {
            var cartItem = new ShoppingCartItems
            {
                Id = Guid.NewGuid(),
                CartId = cartId,
                ProductId = product.Id,
                ProductCount = (product is Bag) ? count : 0,
                LastModified_19118155 = DateTime.Now
            };
            if(product is Shoe)
            {
                cartItem = AddSizes(size, cartItem, count);
            }
            

            return cartItem;
        }

        private async Task<ShoppingCart> CreateCart(string userId)
        {
            var cart = new ShoppingCart
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                IsActive = true,
                ShoppingCartItems = new List<ShoppingCartItems>(),
                LastModified_19118155 = DateTime.Now
            };

            await _context.ShoppingCarts.AddAsync(cart);

            var log = new log_19118155
            {
                Id = Guid.NewGuid(),
                TableName = "ShoppingCarts",
                OperationType = "Insert",
                OperationTime = DateTime.Now
            };

            await _context.log_19118155.AddAsync(log);

            await _context.SaveChangesAsync();

            return cart;
        }

        private ShoppingCartItems AddSizes(string size, ShoppingCartItems cartItem, int count)
        {
            switch (size)
            {
                case "35":
                    cartItem.Size35Count += count;
                    break;
                case "36":
                    cartItem.Size36Count += count;
                    break;
                case "37":
                    cartItem.Size37Count += count;
                    break;
                case "38":
                    cartItem.Size38Count += count;
                    break;
                case "39":
                    cartItem.Size39Count += count;
                    break;
                case "40":
                    cartItem.Size40Count += count;
                    break;
                case "41":
                    cartItem.Size41Count += count;
                    break;
            }

            return cartItem;
        }
        
    }
}

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
    public class RemoveFromCartModel : PageModel
    {
        private readonly VBoutique.Data.ApplicationDbContext _context;

        public RemoveFromCartModel(VBoutique.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ShoppingCartItems ShoppingCartItems { get; set; } = default!;

        
        public async Task<IActionResult> OnGetDelete(Guid id)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var shoppingcart = await _context
                 .ShoppingCarts
                 .Include(x => x.ShoppingCartItems)
                 .ThenInclude(x => x.Product)
                 .FirstOrDefaultAsync(x => x.UserId == userId && x.IsActive == true);

            var cartItem = shoppingcart.ShoppingCartItems.FirstOrDefault(x => x.ProductId == id);

            if(cartItem != null)
            {
                shoppingcart.ShoppingCartItems.Remove(cartItem);

                var log = new log_19118155
                {
                    Id = Guid.NewGuid(),
                    TableName = "ShoppingCartItems",
                    OperationType = "Update",
                    OperationTime = DateTime.Now
                };

                await _context.log_19118155.AddAsync(log);
            }
            await _context.SaveChangesAsync();

            return RedirectToPage("/ShoppingCarts/Info");
        }
    }
}

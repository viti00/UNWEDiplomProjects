using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PartsShop.Data;

namespace PartsShop.Pages.Carts
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly PartsShop.Data.ApplicationDbContext _context;

        public DetailsModel(PartsShop.Data.ApplicationDbContext context)
        {
            _context = context;
        }

      public Cart Cart { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(Guid cartId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Cart cart = null;
            if(cartId != Guid.Empty)
            {
                cart = await _context
                .Carts
                .Include(x => x.CartParts)
                .ThenInclude(x => x.Part)
                .FirstOrDefaultAsync(x => x.Id == cartId);
            }
            else
            {
                cart = await _context
                .Carts
                .Include(x => x.CartParts)
                .ThenInclude(x => x.Part)
                .FirstOrDefaultAsync(x => x.UserId == userId && x.IsOrdered == false);
            }
            

            Cart = cart;
            return Page();
        }

        public async  Task<IActionResult> OnGetCartCount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _context
                .Carts
                .Include(x => x.CartParts)
                .ThenInclude(x => x.Part)
                .FirstOrDefaultAsync(x => x.UserId == userId && x.IsOrdered == false);

            return new JsonResult(cart != null && cart.CartParts.Count > 0 ? cart.CartParts.Count : 0);

        }
    }
}

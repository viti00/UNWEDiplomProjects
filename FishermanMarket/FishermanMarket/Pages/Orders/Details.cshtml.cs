using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FishermanMarket.Data;
using FishermanMarket.Data.Models;
using Microsoft.AspNetCore.Authorization;

namespace FishermanMarket.Pages.Orders
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly FishermanMarket.Data.ApplicationDbContext _context;

        public DetailsModel(FishermanMarket.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Order Order { get; set; } = default!;

        public List<BagProduct> Products { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FirstOrDefaultAsync(m => m.Id == id);
            var products = _context.BagProducts
                .Include(x => x.Product)
                    .ThenInclude(x => x.Category)
                .Include(x => x.Product)
                    .ThenInclude(x => x.Image)
                .Where(x => x.BagId == order.BagId).ToList();

            Products = products;

            if (order == null)
            {
                return NotFound();
            }
            else
            {
                Order = order;
            }
            return Page();
        }
    }
}

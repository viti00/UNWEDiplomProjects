using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VBoutique.Data;

namespace VBoutique.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private readonly VBoutique.Data.ApplicationDbContext _context;

        public DetailsModel(VBoutique.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Product Product { get; set; } = default!;
        public Color Color { get; set; }

        public List<ShoeSize> Sizes { get; set; }

        public string Type { get; set; }


        public async Task<IActionResult> OnGetAsync(Guid? id, string type)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }
            Product? product = null;
            if (type == "Bags")
            {
                product = await _context
                    .Product
                    .OfType<Bag>()
                    .Include(x => x.Color)
                    .Cast<Product>()
                    .FirstOrDefaultAsync(x => x.Id == id);
            }
            else if (type == "Shoes")
            {
                product = await _context
                    .Product
                    .OfType<Shoe>()
                    .Include(x => x.Color)
                    .Include(x => x.ShoeSizes)
                    .ThenInclude(x => x.Size)
                    .Cast<Product>()
                    .FirstOrDefaultAsync(x => x.Id == id);
            }

            if (product == null)
            {
                return NotFound();
            }
            else
            {
                if (product is Bag bag)
                {
                    Color = bag.Color;
                }
                else if (product is Shoe shoe)
                {
                    Color = shoe.Color;
                    Sizes = shoe.ShoeSizes.Where(x=> x.AvailableCount > 0).ToList();
                }
                Type = type;
                Product = product;
            }
            return Page();
        }
    }
}

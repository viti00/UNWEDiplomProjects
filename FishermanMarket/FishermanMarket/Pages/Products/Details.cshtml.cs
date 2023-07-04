using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FishermanMarket.Data;
using FishermanMarket.Data.Models;
using System.Security.Claims;

namespace FishermanMarket.Pages.Products
{
    [IgnoreAntiforgeryToken(Order = 2000)]
    public class DetailsModel : PageModel
    {
        private readonly FishermanMarket.Data.ApplicationDbContext _context;

        public DetailsModel(FishermanMarket.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Product Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(x => x.Image)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                Product = product;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id, int quantity)
        {
            await AddToBag(id, quantity);
            var pr = await _context.Products.Include(x=> x.Image).Include(x=> x.Category).FirstOrDefaultAsync(x => x.Id == id);
            Product = pr;
            return Page();
        }

        public async Task AddToBag(Guid productId, int quantity)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var bag = await _context.Bags
                .Include(x => x.Products)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.Image)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.UserId == userId && x.Status == "O");

            if (bag == null)
            {
                bag = await CreateBag(userId);
            }
            var product = bag.Products.FirstOrDefault(x => x.ProductId == productId);

            if (product != null)
            {
                product.Quantity += quantity;
            }
            else
            {
                product = await CreateBagProduct(productId, bag.Id, quantity);
                var log = new log_19118062
                {
                    Id = Guid.NewGuid(),
                    Table = "BagProducts",
                    Operation = "Insert",
                    Time = DateTime.Now
                };
                await _context.log_19118062.AddAsync(log);
                await _context.BagProducts.AddAsync(product);
            }

            await _context.SaveChangesAsync();
        }

        private async Task<BagProduct> CreateBagProduct(Guid productId, Guid bagId, int quantity)
        {
            var bagPr = new BagProduct
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                BagId = bagId,
                Quantity = quantity,
                LastModified_19118062 = DateTime.Now
            };

            return bagPr;
        }

        public async Task<IActionResult> OnPostDelete(Guid id)
        {
            var product = await _context.Products.Include(x=> x.Category).Include(x => x.Image).FirstOrDefaultAsync(x => x.Id == id);
            var catergory = product.Category.Name;
            if(product != null)
            {
                if(product.Image != null)
                {
                    _context.ProductImages.Remove(product.Image);

                    var log_images = new log_19118062
                    {
                        Id = Guid.NewGuid(),
                        Table = "ProductImages",
                        Operation = "Update",
                        Time = DateTime.Now
                    };
                    _context.log_19118062.Add(log_images);
                }
                

                var log_product = new log_19118062
                {
                    Id = Guid.NewGuid(),
                    Table = "Products",
                    Operation = "Update",
                    Time = DateTime.Now
                };

                _context.log_19118062.Add(log_product);
                _context.Remove(product);

                await _context.SaveChangesAsync();
                
            }
            
            return RedirectToPage("Index", new { filter = catergory });
        }

        private async Task<Bag> CreateBag(string userId)
        {
            var bag = new Bag
            {
                Id = Guid.NewGuid(),
                Status = "O",
                UserId = userId,
                User = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId),
                Products = new List<BagProduct>(),
                LastModified_19118062 = DateTime.Now
            };

            var log = new log_19118062
            {
                Id = Guid.NewGuid(),
                Table = "Bag",
                Operation = "Insert",
                Time = DateTime.Now
            };


            await _context.log_19118062.AddAsync(log);
            await _context.Bags.AddAsync(bag);
            await _context.SaveChangesAsync();

            return bag;
        }
    }
}

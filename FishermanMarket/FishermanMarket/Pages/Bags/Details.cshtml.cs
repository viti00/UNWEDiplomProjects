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
using System.Runtime.ConstrainedExecution;
using Microsoft.AspNetCore.Authorization;

namespace FishermanMarket.Pages.Bags
{
    [IgnoreAntiforgeryToken(Order = 2000)]
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly FishermanMarket.Data.ApplicationDbContext _context;

        public DetailsModel(FishermanMarket.Data.ApplicationDbContext context)
        {
            _context = context;
        }

      public Bag Bag { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var bag = await _context.Bags
                .Include(x=> x.Products)
                .ThenInclude(x=> x.Product)
                .ThenInclude(x=> x.Image)
                .Include(x=> x.User)
                .FirstOrDefaultAsync(x => x.UserId == userId && x.Status == "O");

            Bag = bag;

            return Page();
        }

        public async Task<IActionResult> OnGetEditDetails(int newCount, Guid pId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var bag = await _context.Bags
                .Include(x => x.Products)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.Image)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.UserId == userId && x.Status == "O");

            var pr = bag.Products.FirstOrDefault(x => x.ProductId == pId);

            if(pr != null)
            {
                var log = new log_19118062
                {
                    Id = Guid.NewGuid(),
                    Table = "BagProducts",
                    Operation = "Update",
                    Time = DateTime.Now,
                };

                pr.Quantity = newCount;
                await _context.log_19118062.AddAsync(log);
                await _context.SaveChangesAsync();
            }



            return new JsonResult(pr.Quantity);
        }

        public async Task<IActionResult> OnGetPrice(Guid pId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var bag = await _context.Bags
                .Include(x => x.Products)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.Image)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.UserId == userId && x.Status == "O");
            var pr = bag.Products.FirstOrDefault(x => x.ProductId == pId);

            return new JsonResult(pr.Quantity * pr.Product.Price);
        }
        public async Task<IActionResult> OnGetPriceTotal()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var bag = await _context.Bags
                .Include(x => x.Products)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.Image)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.UserId == userId && x.Status == "O");



            return new JsonResult(bag.Products.Sum(x=> x.Quantity * x.Product.Price));
        }

        public async Task<IActionResult> OnGetCount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var bag = await _context.Bags
                .Include(x => x.Products)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.Image)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.UserId == userId && x.Status == "O");

            return new JsonResult(bag != null? bag.Products.Count:"");
        }

        public async Task<IActionResult> OnPostDelete(Guid pId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var bag = await _context.Bags
                .Include(x => x.Products)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.Image)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.UserId == userId && x.Status == "O");
            var pr = bag.Products.FirstOrDefault(x => x.ProductId == pId);

            if(pr != null)
            {
                bag.Products.Remove(pr);
                var log = new log_19118062
                {
                    Id = Guid.NewGuid(),
                    Table = "BagProducts",
                    Operation = "Update",
                    Time = DateTime.Now
                };
                await _context.log_19118062.AddAsync(log);
                await _context.SaveChangesAsync();
            }
            Bag = bag;
            return Page();
        }
    }
}

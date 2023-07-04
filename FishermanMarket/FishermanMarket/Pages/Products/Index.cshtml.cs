using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FishermanMarket.Data;
using FishermanMarket.Data.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FishermanMarket.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly FishermanMarket.Data.ApplicationDbContext _context;

        public IndexModel(FishermanMarket.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Product> Product { get;set; } = default!;

        public int CurrentPage { get; set; } = 1;

        public int EndPage { get; set; }

        public int NumberOfProducstPerPage = 20;

        public string Filter { get; set; }

        public Sort Sort { get; set; }

        public async Task OnGetAsync(string filter, int currentPage, Sort sort)
        {
            if (_context.Products != null)
            {
                Product = await _context.Products
                .Include(p => p.Category)
                .Include(i => i.Image)
                .ToListAsync();
            }

            if (filter != null)
            {
                Product = Product.Where(x=> x.Category.Name == filter).ToList();
            }

            if (sort == Sort.PriceAsc)
            {
                Product = Product.OrderBy(x => x.Price).ToList();
            }
            else if(sort == Sort.PriceDesc){
                Product = Product.OrderByDescending(x=> x.Price).ToList();
            }

            var lastPage = (int)Math.Ceiling((double)Product.Count / NumberOfProducstPerPage);

            if (currentPage > lastPage)
            {
                if (lastPage == 0)
                {
                    lastPage = 1;
                }
                currentPage = lastPage;
            }
            if (currentPage == 0)
            {
                currentPage = 1;
            }
            EndPage = lastPage;
            Filter = filter;
            Sort = sort;
            CurrentPage = currentPage;
            Product = Product.Skip((CurrentPage - 1) * NumberOfProducstPerPage)
           .Take(NumberOfProducstPerPage).ToList();

        }
    }
}

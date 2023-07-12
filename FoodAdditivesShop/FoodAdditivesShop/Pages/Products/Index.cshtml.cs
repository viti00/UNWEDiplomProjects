using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FoodAdditivesShop.Data;
using System.IO;

namespace FoodAdditivesShop.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly FoodAdditivesShop.Data.ApplicationDbContext _context;

        public IndexModel(FoodAdditivesShop.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Product> Product { get;set; } = default!;

        public int CurrentPage { get; set; } = 1;

        public int TotalPages { get; set; }

        public int ProductsPerPage { get; set; } = 1;

        public string Search { get; set; }

        public Sort Sort { get; set; }

        public async Task OnGetAsync(int currentPage, string search, Sort sort)
        {
            if (_context.Products != null)
            {
                Product = await _context.Products.ToListAsync();
            }

            if (search != null)
            {
                Product = Product
                    .Where(x => x.ProductName.Contains(search, StringComparison.OrdinalIgnoreCase)
                    || x.ProductMake.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            if (sort != Sort.Normal)
            {
                Product = SortProducts(Product.ToList(), sort);
            }

            var totalPages = (int)Math.Ceiling((double)Product.Count / ProductsPerPage);

            if (currentPage > totalPages)
            {
                if (totalPages == 0)
                {
                    totalPages = 1;
                }
                currentPage = totalPages;
            }
            if (currentPage == 0)
            {
                currentPage = 1;
            }

            TotalPages = totalPages;
            CurrentPage = currentPage;
            Sort = sort;
            Search = search;
            Product = Product.Skip((CurrentPage - 1) * ProductsPerPage)
           .Take(ProductsPerPage).ToList();
        }

        private List<Product> SortProducts(List<Product> products, Sort sort)
        {
            switch (sort)
            {
                case Sort.PriceAsc:
                    products = products.OrderBy(x => x.ProductPrice).ToList();
                    break;
                case Sort.PriceDesc:
                    products = products.OrderByDescending(x => x.ProductPrice).ToList();
                    break;
                case Sort.LastModifiedAsc:
                    products = products.OrderByDescending(x => x.LastModified_19118119).ToList();
                    break;
                case Sort.LastModifiedDesc:
                    products = products.OrderBy(x => x.LastModified_19118119).ToList();
                    break;
            }

            return products;
        }
    }
}

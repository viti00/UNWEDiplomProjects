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
    public class IndexModel : PageModel
    {
        private readonly VBoutique.Data.ApplicationDbContext _context;

        public IndexModel(VBoutique.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Product> Product { get; set; } = default!;

        [BindProperty]
        public string[] Colors { get; set; }

        [BindProperty]
        public string[] Makes { get; set; }

        public string Type { get; set; }

        public string[]? SelectedColors { get; set; }

        public string[]? SelectedMakes { get; set; }

        public int ProductsPerPage { get; set; } = 9;

        public int LastPage { get; set; }

        public int CurrPage { get; set; }

        public Sort Sort { get; set; }


        public async Task OnGetAsync(string type, string[] selectedColors, string[] selectedMakes, int currPage, Sort sort)
        {
            if (_context.Product != null)
            {
                if (type != null && type == "Bags")
                {
                    Product = await _context
                    .Product
                    .OfType<Bag>()
                    .Include(x => x.Color)
                    .Cast<Product>()
                    .ToListAsync();
                }
                else if (type != null && type == "Shoes")
                {
                    Product = await _context
                    .Product
                    .OfType<Shoe>()
                    .Include(x => x.Color)
                    .Include(x => x.ShoeSizes)
                    .ThenInclude(x => x.Size)
                    .Cast<Product>()
                    .ToListAsync();
                }

                Product = FilterProducts(Product.ToList(), selectedColors, selectedMakes, type);
                Product = SortProducts(Product.ToList(), sort);

                var totalPages = (int)Math.Ceiling((double)Product.Count / ProductsPerPage);

                if (currPage > totalPages)
                {
                    if (totalPages == 0)
                    {
                        totalPages = 1;
                    }
                    currPage = totalPages;
                }
                if (currPage == 0)
                {
                    currPage = 1;
                }

                
                Type = type;
                Sort = sort;
                SelectedMakes = selectedMakes;
                SelectedColors = selectedColors;
                Makes = GetMakes(type);
                Colors = await _context.Colors.Select(x => x.ColorNameBg).ToArrayAsync();
                CurrPage = currPage;
                LastPage = totalPages;
                Product = Product.Skip((CurrPage - 1) * ProductsPerPage)
               .Take(ProductsPerPage).ToList();
            }
        }

        private string[] GetMakes(string type)
        {
            string[] makes = null;
            if (type == "Bags")
            {
                makes = _context.Product.OfType<Bag>().Select(x => x.Make).Distinct().ToArray();
            }
            else if (type == "Shoes")
            {
                makes = _context.Product.OfType<Shoe>().Select(x => x.Make).Distinct().ToArray();
            }

            return makes;
        }

        private List<Product> FilterProducts(List<Product> products, string[] selectedColors, string[] selectedMakes, string type)
        {
            if (selectedColors.Length > 0)
            {
                if (type == "Bags")
                {
                    products = products.OfType<Bag>().Where(x => selectedColors.Contains(x.Color.ColorNameBg)).Cast<Product>().ToList();
                }
                else if (type == "Shoes")
                {
                    products = products.OfType<Shoe>().Where(x => selectedColors.Contains(x.Color.ColorNameBg)).Cast<Product>().ToList();

                }
            }
            if (selectedMakes.Length > 0)
            {
                products = products.Where(x => selectedMakes.Contains(x.Make)).ToList();
            }

            return products;
        }

        private List<Product> SortProducts(List<Product> products, Sort sort)
        {
            if(sort != Sort.Default)
            {
                if (sort == Sort.PriceAsc)
                {
                    products = products.OrderBy(x => x.Price).ToList();
                }
                else if (sort == Sort.PriceDesc)
                {
                    products = products.OrderByDescending(x => x.Price).ToList();
                }
                else if (sort == Sort.DateNewest)
                {
                    products = products.OrderByDescending(x => x.LastModified_19118155).ToList();
                }
                else if (sort == Sort.DateOldest)
                {
                    products = products.OrderBy(x => x.LastModified_19118155).ToList();
                }
            }
            return products;
        }   
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FoodAdditivesShop.Data;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace FoodAdditivesShop.Pages.Products
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly FoodAdditivesShop.Data.ApplicationDbContext _context;

        public CreateModel(FoodAdditivesShop.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            Product.Id = Guid.NewGuid();
            Product.DateOfAdd = DateTime.Now;
            Product.LastModified_19118119 = DateTime.Now;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _context.Products.AddAsync(Product);

            var log = new log_19118119
            {
                Id = Guid.NewGuid(),
                Table = "Products",
                Action = "Insert",
                ActionTime = DateTime.Now,
            };

            await _context.log_19118119.AddAsync(log);

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

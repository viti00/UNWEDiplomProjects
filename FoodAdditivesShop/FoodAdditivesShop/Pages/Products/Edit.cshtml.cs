using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodAdditivesShop.Data;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace FoodAdditivesShop.Pages.Products
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly FoodAdditivesShop.Data.ApplicationDbContext _context;

        public EditModel(FoodAdditivesShop.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Product Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product =  await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            Product = product;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            Product.LastModified_19118119 = DateTime.Now;
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Product).State = EntityState.Modified;


            var log = new log_19118119
            {
                Id = Guid.NewGuid(),
                Table = "Products",
                Action = "Update",
                ActionTime = DateTime.Now,
            };

            await _context.log_19118119.AddAsync(log);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(Product.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }


        public async Task<IActionResult> OnGetDelete(Guid partId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == partId);

            if (product != null)
            {
                _context.Products.Remove(product);

                var log = new log_19118119
                {
                    Id = Guid.NewGuid(),
                    Table = "Products",
                    Action = "Update",
                    ActionTime = DateTime.Now,
                };

                await _context.log_19118119.AddAsync(log);

                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

        private bool ProductExists(Guid id)
        {
          return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

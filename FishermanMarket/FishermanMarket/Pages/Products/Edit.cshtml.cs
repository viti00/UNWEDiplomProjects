using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FishermanMarket.Data;
using FishermanMarket.Data.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace FishermanMarket.Pages.Products
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly FishermanMarket.Data.ApplicationDbContext _context;

        public EditModel(FishermanMarket.Data.ApplicationDbContext context)
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

            var product =  await _context.Products.Include(x=> x.Image).FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            Product = product;
           ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync([FromForm] IFormFile file)
        {
            var image = await _context.ProductImages.FirstOrDefaultAsync(x => x.ProductId == Product.Id);
            if (image != null)
            {
                var log_image = new log_19118062
                {
                    Id = Guid.NewGuid(),
                    Table = "ProductImages",
                    Operation = "Update",
                    Time = DateTime.Now
                };
                _context.log_19118062.Add(log_image);
                
                _context.ProductImages.Remove(image);
            }

            Product.Image = await CreateImage(file);
            Product.LastModified_19118062 = DateTime.Now;

            if (!ModelState.IsValid)
            {
                ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
                return Page();
            }

           

            var log_image_insert = new log_19118062
            {
                Id = Guid.NewGuid(),
                Table = "ProductImages",
                Operation = "Insert",
                Time = DateTime.Now
            };
            _context.log_19118062.Add(log_image_insert);

            var log_pr = new log_19118062
            {
                Id = Guid.NewGuid(),
                Table = "Products",
                Operation = "Update",
                Time = DateTime.Now
            };
            _context.log_19118062.Add(log_pr);

            _context.Attach(Product).State = EntityState.Modified;

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
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == Product.CategoryId);

            return RedirectToPage("./Index",new {filter = category.Name});
        }

        private bool ProductExists(Guid id)
        {
          return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private async Task<ProductImage> CreateImage(IFormFile file)
        {
            ProductImage img = new ProductImage();
            if (file != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);

                    if (memoryStream.Length < 2097152)
                    {
                        var image = new ProductImage()
                        {
                            Bytes = memoryStream.ToArray(),
                            Description = file.FileName,
                            FileExtension = Path.GetExtension(file.FileName),
                            Size = file.Length,
                            LastModified_19118062 = DateTime.Now
                        };

                        img = image;
                    }
                }

            }

            return img;
        }
    }
}

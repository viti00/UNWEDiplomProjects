using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FishermanMarket.Data;
using FishermanMarket.Data.Models;
using System.IO.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace FishermanMarket.Pages.Products
{
    [Authorize(Roles ="Admin")]

    public class CreateModel : PageModel
    {
        private readonly FishermanMarket.Data.ApplicationDbContext _context;

        public CreateModel(FishermanMarket.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync([FromForm] IFormFile file)
        {
            Product.Id = Guid.NewGuid();


            if(file == null)
            {
                ModelState.AddModelError("File", "Снимката е задължителна!");
            }
            if (!ModelState.IsValid || _context.Products == null || Product == null)
            {
                ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
                return Page();
            }

            Product.Image = await CreateImage(file);

            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == Product.CategoryId);

            var log_pr = new log_19118062
            {
                Id = Guid.NewGuid(),
                Table = "Products",
                Operation = "Insert",
                Time = DateTime.Now
            };

            var log_img = new log_19118062
            {
                Id = Guid.NewGuid(),
                Table = "ProductImages",
                Operation = "Insert",
                Time = DateTime.Now
            };



            _context.Products.Add(Product);
            _context.log_19118062.Add(log_pr);
            _context.log_19118062.Add(log_img);

            await _context.SaveChangesAsync();

            return RedirectToPage($"./Index", new{filter=category.Name });
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

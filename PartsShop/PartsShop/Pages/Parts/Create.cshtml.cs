using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PartsShop.Data;

namespace PartsShop.Pages.Parts
{
    [Authorize(Roles ="Admin")]
    public class CreateModel : PageModel
    {
        private readonly PartsShop.Data.ApplicationDbContext _context;

        public CreateModel(PartsShop.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName");
            return Page();
        }

        [BindProperty]
        public Part Part { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            Part.Id = Guid.NewGuid();
            Part.DateOfAdd = DateTime.Now;
            Part.LastModified_19118073 = DateTime.Now;

            if (!ModelState.IsValid || _context.Parts == null || Part == null)
            {
                ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName");
                return Page();
            }

            await _context.Parts.AddAsync(Part);

            var log = new log_19118073
            {
                Id = Guid.NewGuid(),
                Table = "Parts",
                Action = "Insert",
                ActionTime = DateTime.Now,
            };

            await _context.log_19118073.AddAsync(log);

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

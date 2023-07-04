using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using HomeEssentials.Data;
using Microsoft.AspNetCore.Authorization;

namespace HomeEssentials.Pages.Items
{
    [Authorize(Roles ="Administrator")]
    public class CreateModel : PageModel
    {
        private readonly HomeEssentials.Data.ApplicationDbContext _context;

        public CreateModel(HomeEssentials.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["CategoryId"] = new SelectList(_context.ItemCategories, "Id", "Category");
            return Page();
        }

        [BindProperty]
        public Item Item { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            Item.Id = Guid.NewGuid();
            Item.CreatedDate = DateTime.Now;
            Item.LastModified_19118075 = DateTime.Now;
            Item.IsActive = true;

            if (!ModelState.IsValid || _context.Items == null || Item == null)
            {
                ViewData["CategoryId"] = new SelectList(_context.ItemCategories, "Id", "Category");
                return Page();
            }

            var log = new log_19118075
            {
                Id = Guid.NewGuid(),
                Table = "Items",
                Command = "Insert",
                DateTime = DateTime.Now
            };

            await _context.log_19118075.AddAsync(log);

            await _context.Items.AddAsync(Item);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

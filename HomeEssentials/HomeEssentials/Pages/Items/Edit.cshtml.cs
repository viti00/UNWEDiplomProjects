using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HomeEssentials.Data;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace HomeEssentials.Pages.Items
{
    [Authorize(Roles = "Administrator")]
    public class EditModel : PageModel
    {
        private readonly HomeEssentials.Data.ApplicationDbContext _context;

        public EditModel(HomeEssentials.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Item Item { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id, string type)
        {
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var item =  await _context.Items.FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            Item = item;

            if(type != null && type == "delete")
            {
                await MakeItemNotActive(Item);
                return RedirectToPage("./Index");
            }

            ViewData["CategoryId"] = new SelectList(_context.ItemCategories, "Id", "Category");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.

        public async Task MakeItemNotActive(Item item)
        {
            item.IsActive = false;
            item.LastModified_19118075 = DateTime.Now;

            var log = new log_19118075
            {
                Id = Guid.NewGuid(),
                Table = "Items",
                Command = "Update",
                DateTime = DateTime.Now
            };

            await _context.log_19118075.AddAsync(log);

            await _context.SaveChangesAsync();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            Item.LastModified_19118075 = DateTime.Now;

            if (!ModelState.IsValid)
            {
                ViewData["CategoryId"] = new SelectList(_context.ItemCategories, "Id", "Category");
                return Page();
            }

            _context.Attach(Item).State = EntityState.Modified;

            var log = new log_19118075
            {
                Id = Guid.NewGuid(),
                Table = "Items",
                Command = "Update",
                DateTime = DateTime.Now
            };

            await _context.log_19118075.AddAsync(log);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(Item.Id))
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

        private bool ItemExists(Guid id)
        {
          return (_context.Items?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

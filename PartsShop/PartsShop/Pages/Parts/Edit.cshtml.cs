using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PartsShop.Data;

namespace PartsShop.Pages.Parts
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly PartsShop.Data.ApplicationDbContext _context;

        public EditModel(PartsShop.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Part Part { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.Parts == null)
            {
                return NotFound();
            }

            var part =  await _context.Parts.FirstOrDefaultAsync(m => m.Id == id);
            if (part == null)
            {
                return NotFound();
            }
            Part = part;
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            Part.LastModified_19118073 = DateTime.Now;
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Part).State = EntityState.Modified;


            var log = new log_19118073
            {
                Id = Guid.NewGuid(),
                Table = "Parts",
                Action = "Update",
                ActionTime = DateTime.Now,
            };

            await _context.log_19118073.AddAsync(log);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PartExists(Part.Id))
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
            var part = await _context.Parts.FirstOrDefaultAsync(x => x.Id == partId);

            if (part != null)
            {
                _context.Parts.Remove(part);

                var log = new log_19118073
                {
                    Id = Guid.NewGuid(),
                    Table = "Parts",
                    Action = "Update",
                    ActionTime = DateTime.Now,
                };

                await _context.log_19118073.AddAsync(log);

                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

        private bool PartExists(Guid id)
        {
          return (_context.Parts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

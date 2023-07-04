using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VBoutique.Data;

namespace VBoutique.Pages.Bags
{
    [Authorize(Roles = "Admin, ProductsManager")]
    public class EditModel : PageModel
    {
        private readonly VBoutique.Data.ApplicationDbContext _context;

        public EditModel(VBoutique.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Bag Bag { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.Bags == null)
            {
                return NotFound();
            }

            var bag =  await _context.Bags.FirstOrDefaultAsync(m => m.Id == id);
            if (bag == null)
            {
                return NotFound();
            }
            Bag = bag;
            ViewData["ColorId"] = new SelectList(_context.Colors, "Id", "ColorNameBg");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            Bag.LastModified_19118155 = DateTime.Now;
            if (!ModelState.IsValid)
            {
                ViewData["ColorId"] = new SelectList(_context.Colors, "Id", "ColorNameBg");
                return Page();
            }

            _context.Attach(Bag).State = EntityState.Modified;

            var log = new log_19118155
            {
                Id = Guid.NewGuid(),
                TableName = "Bags",
                OperationType = "Update",
                OperationTime = DateTime.Now,
            };

            await _context.log_19118155.AddAsync(log);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BagExists(Bag.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("/Products/Details", new {id=Bag.Id, type="Bags"});
        }

        public async Task<IActionResult> OnGetDeleteProduct(Guid id)
        {
            var product = await _context.Product.FirstOrDefaultAsync(x => x.Id == id);

            _context.Product.Remove(product);
            var log = new log_19118155
            {
                Id = Guid.NewGuid(),
                TableName = "Bags",
                OperationType = "Update",
                OperationTime = DateTime.Now,
            };

            await _context.log_19118155.AddAsync(log);

            await _context.SaveChangesAsync();

            return RedirectToPage("/Products/Index", new { type = "Bags" });

        }

        private bool BagExists(Guid id)
        {
          return (_context.Bags?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using VBoutique.Data;

namespace VBoutique.Pages.Bags
{
    [Authorize(Roles ="Admin, ProductsManager")]
    public class CreateModel : PageModel
    {
        private readonly VBoutique.Data.ApplicationDbContext _context;

        public CreateModel(VBoutique.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["ColorId"] = new SelectList(_context.Colors, "Id", "ColorNameBg");
            return Page();
        }

        [BindProperty]
        public Bag Bag { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            Bag.Id = Guid.NewGuid();
            Bag.LastModified_19118155 = DateTime.Now;

            if (!ModelState.IsValid || _context.Bags == null || Bag == null)
            {
                ViewData["ColorId"] = new SelectList(_context.Colors, "Id", "ColorNameBg");
                return Page();
            }

            await _context.Bags.AddAsync(Bag);

            var log = new log_19118155
            {
                Id = Guid.NewGuid(),
                TableName = "Bags",
                OperationType = "Insert",
                OperationTime = DateTime.Now,
            };

            await _context.log_19118155.AddAsync(log);

            await _context.SaveChangesAsync();

            return RedirectToPage("/Products/Index", new {type="Bags"});
        }
    }
}

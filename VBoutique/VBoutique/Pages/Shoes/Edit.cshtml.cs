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

namespace VBoutique.Pages.Shoes
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
        public Shoe Shoe { get; set; } = default!;

        [BindProperty]
        public int Size35Count { get; set; }
        [BindProperty]
        public int Size36Count { get; set; }
        [BindProperty]
        public int Size37Count { get; set; }
        [BindProperty]
        public int Size38Count { get; set; }
        [BindProperty]
        public int Size39Count { get; set; }
        [BindProperty]
        public int Size40Count { get; set; }
        [BindProperty]
        public int Size41Count { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.Shoes == null)
            {
                return NotFound();
            }

            var shoe =  await _context
                .Shoes
                .Include(x=> x.ShoeSizes)
                .ThenInclude(x=> x.Size)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (shoe == null)
            {
                return NotFound();
            }
            Shoe = shoe;
            Size35Count = Shoe.ShoeSizes.FirstOrDefault(x => x.Size.Value == "35").AvailableCount;
            Size36Count = Shoe.ShoeSizes.FirstOrDefault(x => x.Size.Value == "36").AvailableCount;
            Size37Count = Shoe.ShoeSizes.FirstOrDefault(x => x.Size.Value == "37").AvailableCount;
            Size38Count = Shoe.ShoeSizes.FirstOrDefault(x => x.Size.Value == "38").AvailableCount;
            Size39Count = Shoe.ShoeSizes.FirstOrDefault(x => x.Size.Value == "39").AvailableCount;
            Size40Count = Shoe.ShoeSizes.FirstOrDefault(x => x.Size.Value == "40").AvailableCount;
            Size41Count = Shoe.ShoeSizes.FirstOrDefault(x => x.Size.Value == "41").AvailableCount;

            ViewData["ColorId"] = new SelectList(_context.Colors, "Id", "ColorNameBg");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            Shoe.LastModified_19118155 = DateTime.Now;
            if (!ModelState.IsValid)
            {
                Size35Count = Shoe.ShoeSizes.FirstOrDefault(x => x.Size.Value == "35").AvailableCount;
                Size36Count = Shoe.ShoeSizes.FirstOrDefault(x => x.Size.Value == "36").AvailableCount;
                Size37Count = Shoe.ShoeSizes.FirstOrDefault(x => x.Size.Value == "37").AvailableCount;
                Size38Count = Shoe.ShoeSizes.FirstOrDefault(x => x.Size.Value == "38").AvailableCount;
                Size39Count = Shoe.ShoeSizes.FirstOrDefault(x => x.Size.Value == "39").AvailableCount;
                Size40Count = Shoe.ShoeSizes.FirstOrDefault(x => x.Size.Value == "40").AvailableCount;
                Size41Count = Shoe.ShoeSizes.FirstOrDefault(x => x.Size.Value == "41").AvailableCount;
                ViewData["ColorId"] = new SelectList(_context.Colors, "Id", "ColorNameBg");
                return Page();
            }

            await CheckForUpdate(_context.ShoeSizes.Include(x=> x.Size).Where(x=> x.ShoeId == Shoe.Id).ToList());

            _context.Attach(Shoe).State = EntityState.Modified;
            var log = new log_19118155
            {
                Id = Guid.NewGuid(),
                TableName = "Shoes",
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
                if (!ShoeExists(Shoe.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("/Products/Details", new { id = Shoe.Id, type = "Shoes" });
        }

        public async Task<IActionResult> OnGetDeleteProduct(Guid id)
        {
            var product = await _context.Product.FirstOrDefaultAsync(x => x.Id == id);

            _context.Product.Remove(product);
            var log = new log_19118155
            {
                Id = Guid.NewGuid(),
                TableName = "Shoes",
                OperationType = "Update",
                OperationTime = DateTime.Now,
            };

            await _context.log_19118155.AddAsync(log);

            await _context.SaveChangesAsync();

            return RedirectToPage("/Products/Index", new { type = "Shoes" });

        }

        private bool ShoeExists(Guid id)
        {
          return (_context.Shoes?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private async Task CheckForUpdate(List<ShoeSize> sizes)
        {
            var last35Count = sizes.FirstOrDefault(x => x.Size.Value == "35").AvailableCount;
            var last36Count = sizes.FirstOrDefault(x => x.Size.Value == "36").AvailableCount;
            var last37Count = sizes.FirstOrDefault(x => x.Size.Value == "37").AvailableCount;
            var last38Count = sizes.FirstOrDefault(x => x.Size.Value == "38").AvailableCount;
            var last39Count = sizes.FirstOrDefault(x => x.Size.Value == "39").AvailableCount;
            var last40Count = sizes.FirstOrDefault(x => x.Size.Value == "40").AvailableCount;
            var last41Count = sizes.FirstOrDefault(x => x.Size.Value == "41").AvailableCount;

            var updatedCount = 0;

            if (last35Count != Size35Count)
            {
                sizes.FirstOrDefault(x => x.Size.Value == "35").AvailableCount = Size35Count;
                updatedCount++;
            }
            else if (last36Count != Size36Count)
            {
                sizes.FirstOrDefault(x => x.Size.Value == "36").AvailableCount = Size36Count;
                updatedCount++;
            }
            else if (last37Count != Size37Count)
            {
                sizes.FirstOrDefault(x => x.Size.Value == "37").AvailableCount = Size37Count;
                updatedCount++;
            }
            else if (last38Count != Size38Count)
            {
                sizes.FirstOrDefault(x => x.Size.Value == "38").AvailableCount = Size38Count;
                updatedCount++;
            }
            else if (last39Count != Size39Count)
            {
                sizes.FirstOrDefault(x => x.Size.Value == "39").AvailableCount = Size39Count;
                updatedCount++;
            }
            else if (last40Count != Size40Count)
            {
                sizes.FirstOrDefault(x => x.Size.Value == "40").AvailableCount = Size40Count;
                updatedCount++;
            }
            else if (last41Count != Size41Count)
            {
                sizes.FirstOrDefault(x => x.Size.Value == "41").AvailableCount = Size41Count;
                updatedCount++;
            }

            for (int i = 0; i < updatedCount; i++)
            {
                var log = new log_19118155
                {
                    Id = Guid.NewGuid(),
                    TableName = "ShoeSizes",
                    OperationType = "Update",
                    OperationTime = DateTime.Now,
                };

                await _context.log_19118155.AddAsync(log);
            }
        }
    }
}

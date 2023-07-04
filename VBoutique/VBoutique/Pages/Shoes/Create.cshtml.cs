using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using VBoutique.Data;

namespace VBoutique.Pages.Shoes
{
    [Authorize(Roles = "Admin, ProductsManager")]
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

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            Shoe.Id = Guid.NewGuid();
            Shoe.LastModified_19118155 = DateTime.Now;
            Shoe.ShoeSizes = await AddSizes(Shoe.Id);

            if (!ModelState.IsValid || _context.Shoes == null || Shoe == null)
            {
                var err = ModelState.Values.SelectMany(x => x.Errors.Select(x => x.ErrorMessage)).ToList();
                ViewData["ColorId"] = new SelectList(_context.Colors, "Id", "ColorNameBg");
                return Page();
            }

            await _context.Shoes.AddAsync(Shoe);
            var log = new log_19118155
            {
                Id = Guid.NewGuid(),
                TableName = "Shoes",
                OperationType = "Insert",
                OperationTime = DateTime.Now,
            };

            await _context.log_19118155.AddAsync(log);

            await _context.SaveChangesAsync();

            return RedirectToPage("/Products/Index", new { type = "Shoes" });
        }

        private async Task<List<ShoeSize>> AddSizes(Guid shoeId)
        {
            var shoeSizes = new List<ShoeSize>()
            {
                new ShoeSize {ShoeId = shoeId, SizeId = _context.Sizes.FirstOrDefault(x=> x.Value == "35").Id, AvailableCount = Size35Count, LastModified_19118155 = DateTime.Now},
                new ShoeSize {ShoeId = shoeId, SizeId = _context.Sizes.FirstOrDefault(x=> x.Value == "36").Id, AvailableCount = Size36Count, LastModified_19118155 = DateTime.Now},
                new ShoeSize {ShoeId = shoeId, SizeId = _context.Sizes.FirstOrDefault(x=> x.Value == "37").Id, AvailableCount = Size37Count, LastModified_19118155 = DateTime.Now},
                new ShoeSize {ShoeId = shoeId, SizeId = _context.Sizes.FirstOrDefault(x=> x.Value == "38").Id, AvailableCount = Size38Count, LastModified_19118155 = DateTime.Now},
                new ShoeSize {ShoeId = shoeId, SizeId = _context.Sizes.FirstOrDefault(x=> x.Value == "39").Id, AvailableCount = Size39Count, LastModified_19118155 = DateTime.Now},
                new ShoeSize {ShoeId = shoeId, SizeId = _context.Sizes.FirstOrDefault(x=> x.Value == "40").Id, AvailableCount = Size40Count, LastModified_19118155 = DateTime.Now},
                new ShoeSize {ShoeId = shoeId, SizeId = _context.Sizes.FirstOrDefault(x=> x.Value == "41").Id, AvailableCount = Size41Count, LastModified_19118155 = DateTime.Now},
            };

            await _context.AddRangeAsync(shoeSizes);

            foreach (var item in shoeSizes)
            {
                var log = new log_19118155
                {
                    Id = Guid.NewGuid(),
                    TableName = "ShoeSizes",
                    OperationType = "Insert",
                    OperationTime = DateTime.Now,
                };

                await _context.log_19118155.AddAsync(log);
            }

            return shoeSizes;
        }
    }
}

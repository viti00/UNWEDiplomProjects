using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PartsShop.Data;

namespace PartsShop.Pages.PartReviews
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly PartsShop.Data.ApplicationDbContext _context;

        public CreateModel(PartsShop.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(Guid partId)
        {
            ViewData["UserId"] = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["PartId"] = partId;
            return Page();
        }

        [BindProperty]
        public PartReview PartReview { get; set; } = default!;



        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            PartReview.Id = Guid.NewGuid();
            PartReview.LastModified_19118073 = DateTime.Now;
            if (!ModelState.IsValid || _context.PartReviews == null || PartReview == null)
            {
                ViewData["UserId"] = User.FindFirstValue(ClaimTypes.NameIdentifier);
                ViewData["PartId"] = PartReview.PartId;
                return Page();
            }

            await _context.PartReviews.AddAsync(PartReview);

            var log = new log_19118073
            {
                Id = Guid.NewGuid(),
                Table = "PartReviews",
                Action = "Insert",
                ActionTime = DateTime.Now,
            };

            await _context.log_19118073.AddAsync(log);

            await _context.SaveChangesAsync();

            return RedirectToPage("/Parts/Details", new {id=PartReview.PartId });
        }
    }
}

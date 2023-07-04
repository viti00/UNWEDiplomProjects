using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PartsShop.Data;

namespace PartsShop.Pages.PartReviews
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly PartsShop.Data.ApplicationDbContext _context;

        public EditModel(PartsShop.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PartReview PartReview { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.PartReviews == null)
            {
                return NotFound();
            }

            var partreview =  await _context.PartReviews.FirstOrDefaultAsync(m => m.Id == id);
            if (partreview == null)
            {
                return NotFound();
            }
            PartReview = partreview;
            ViewData["UserId"] = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            PartReview.LastModified_19118073 = DateTime.Now;
            if (!ModelState.IsValid)
            {
                ViewData["UserId"] = User.FindFirstValue(ClaimTypes.NameIdentifier);
                return Page();
            }

            var log = new log_19118073
            {
                Id = Guid.NewGuid(),
                Table = "PartReviews",
                Action = "Update",
                ActionTime = DateTime.Now,
            };

            await _context.log_19118073.AddAsync(log);

            _context.Attach(PartReview).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PartReviewExists(PartReview.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("/Parts/Details", new { id = PartReview.PartId });
        }

        public async Task<IActionResult> OnGetDelete(Guid reviewId)
        {
            var review = await _context.PartReviews.FirstOrDefaultAsync(x => x.Id == reviewId);
            var partId = review.PartId;
            if (review != null)
            {
                _context.PartReviews.Remove(review);

                var log = new log_19118073
                {
                    Id = Guid.NewGuid(),
                    Table = "PartReviews",
                    Action = "Update",
                    ActionTime = DateTime.Now,
                };

                await _context.log_19118073.AddAsync(log);

                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Parts/Details", new { id = partId });
        }

        private bool PartReviewExists(Guid id)
        {
          return (_context.PartReviews?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

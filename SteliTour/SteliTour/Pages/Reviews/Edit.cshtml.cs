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
using SteliTour.Data;

namespace SteliTour.Pages.Reviews
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly SteliTour.Data.ApplicationDbContext _context;

        public EditModel(SteliTour.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Review Review { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id, string handler)
        {
            if (id == null || _context.Review == null)
            {
                return NotFound();
            }

            var review =  await _context.Review.FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }
            Review = review;

            if (handler != null && handler == "Delete")
            {
                await Delete(Review.Id);
                return RedirectToPage("./Index");
            }

            ViewData["UserId"] = _context.Users.FirstOrDefault(x => x.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).Id;
            return Page();
        }

        public async Task Delete(Guid id)
        {
            var rev = await _context.Review.FirstOrDefaultAsync(x => x.Id == id);

            if (rev != null)
            {
                _context.Review.Remove(rev);

                var log = new log_19118105
                {
                    Id = Guid.NewGuid(),
                    Table = "Reviews",
                    Operation = "Update",
                    Date = DateTime.Now,
                };

                await _context.log_19118105.AddAsync(log);

                await _context.SaveChangesAsync();
            }
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (Review.Rating <= 0 || Review.Rating > 5)
            {
                ModelState.AddModelError("StarRating", "Оценката е задължителна");
            }
            if (!ModelState.IsValid)
            {
                ViewData["UserId"] = _context.Users.FirstOrDefault(x => x.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).Id;
                return Page();
            }

            _context.Attach(Review).State = EntityState.Modified;

            Review.LastModified_19118105 = DateTime.Now;

            var log = new log_19118105
            {
                Id = Guid.NewGuid(),
                Table = "Reviews",
                Operation = "Update",
                Date = DateTime.Now
            };
            await _context.log_19118105.AddAsync(log);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(Review.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index", new { type = "my" });
        }

        private bool ReviewExists(Guid id)
        {
          return (_context.Review?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

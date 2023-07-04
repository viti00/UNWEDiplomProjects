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

namespace HomeEssentials.Pages.Reviews
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly HomeEssentials.Data.ApplicationDbContext _context;

        public EditModel(HomeEssentials.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Review Review { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.Reviews == null)
            {
                return NotFound();
            }

            var review =  await _context.Reviews
                .Include(x=> x.Item)
                .Include(x=> x.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }
            Review = review;
            int[] grades = { 1, 2, 3, 4, 5 };
            ViewData["Grade"] = new SelectList(grades);
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.

        public async Task<IActionResult> OnGetDelete(Guid id)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(x => x.Id == id);
            var itemId = review?.ItemId;
            if (review != null)
            {
                
                _context.Reviews.Remove(review);

                var log = new log_19118075
                {
                    Id = Guid.NewGuid(),
                    Table = "Reviews",
                    Command = "Update",
                    DateTime = DateTime.Now
                };

                await _context.log_19118075.AddAsync(log);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Items/Details", new { id = itemId });
        }
        public async Task<IActionResult> OnPostAsync()
        {
            Review.LastModified_19118075 = DateTime.Now;
            if (!ModelState.IsValid)
            {
                var err = ModelState.Values.SelectMany(x => x.Errors.Select(x => x.ErrorMessage)).ToList();
                int[] grades = { 1, 2, 3, 4, 5 };
                ViewData["Grade"] = new SelectList(grades);
                return Page();
            }

            _context.Attach(Review).State = EntityState.Modified;

            var log = new log_19118075
            {
                Id = Guid.NewGuid(),
                Table = "Reviews",
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
                if (!ReviewExists(Review.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("/Reviews/Index", new { id = Review.ItemId });
        }

        private bool ReviewExists(Guid id)
        {
          return (_context.Reviews?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

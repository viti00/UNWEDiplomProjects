using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SteliTour.Data;

namespace SteliTour.Pages.Reviews
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly SteliTour.Data.ApplicationDbContext _context;

        public CreateModel(SteliTour.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["UserId"] = _context.Users.FirstOrDefault(x => x.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).Id;
            return Page();
        }

        [BindProperty]
        public Review Review { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            Review.Id = Guid.NewGuid();
            Review.ReviewDate = DateTime.Now;
            Review.LastModified_19118105 = DateTime.Now;

            if(Review.Rating <= 0 || Review.Rating > 5)
            {
                ModelState.AddModelError("StarRating", "Оценката е задължителна");
            }
            if (!ModelState.IsValid || _context.Review == null || Review == null)
            {
                var errors = ModelState.Values.SelectMany(x => x.Errors.Select(x => x.ErrorMessage)).ToList();
                ViewData["UserId"] = _context.Users.FirstOrDefault(x => x.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).Id;
                return Page();
            }

            var log = new log_19118105
            {
                Id = Guid.NewGuid(),
                Table = "Reviews",
                Operation = "Insert",
                Date = DateTime.Now
            };
            await _context.log_19118105.AddAsync(log);
            await _context.Review.AddAsync(Review);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index", new { type = "my" });
        }
    }
}

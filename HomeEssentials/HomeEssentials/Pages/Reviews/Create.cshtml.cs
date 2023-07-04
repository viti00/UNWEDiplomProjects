using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using HomeEssentials.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace HomeEssentials.Pages.Reviews
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly HomeEssentials.Data.ApplicationDbContext _context;

        public CreateModel(HomeEssentials.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            int[] grades = { 1, 2, 3, 4, 5 };
            ViewData["Grade"] = new SelectList(grades);
            ViewData["ItemId"] = _context.Items.FirstOrDefault(x => x.Id == id).Id;
            ViewData["UserId"] = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Page();
        }

        [BindProperty]
        public Review Review { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(Guid id)
        {

            Review.Id = Guid.NewGuid();
            Review.CreatedDate = DateTime.Now;
            Review.LastModified_19118075 = DateTime.Now;

            if (!ModelState.IsValid || _context.Reviews == null || Review == null)
            {

                int[] grades = { 1, 2, 3, 4, 5 };
                ViewData["Grade"] = new SelectList(grades);
                ViewData["ItemId"] = _context.Items.FirstOrDefault(x => x.Id == id).Id;
                ViewData["UserId"] = User.FindFirstValue(ClaimTypes.NameIdentifier);
                return Page();
            }

            var log = new log_19118075
            {
                Id = Guid.NewGuid(),
                Table = "Reviews",
                Command = "Insert",
                DateTime = DateTime.Now
            };

            await _context.log_19118075.AddAsync(log);

            _context.Reviews.Add(Review);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Reviews/Index", new { id = id });
        }
    }
}

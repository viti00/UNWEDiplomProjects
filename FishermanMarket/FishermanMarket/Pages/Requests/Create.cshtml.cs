using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FishermanMarket.Data;
using FishermanMarket.Data.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FishermanMarket.Pages.Requests
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly FishermanMarket.Data.ApplicationDbContext _context;

        public CreateModel(FishermanMarket.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["UserId"] = _context.Users.FirstOrDefault(x => x.Id == userId).Id;
            return Page();
        }

        [BindProperty]
        public Request Request { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            Request.LastModified_19118062 = DateTime.Now;
            Request.Id = Guid.NewGuid();
            Request.RequestDate = DateTime.Now;
            Request.Status = "Чака отговор";

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var log_req = new log_19118062
            {
                Id = Guid.NewGuid(),
                Table = "Request",
                Operation = "Insert",
                Time = DateTime.Now
            };

            _context.log_19118062.Add(log_req);
            _context.Requests.Add(Request);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

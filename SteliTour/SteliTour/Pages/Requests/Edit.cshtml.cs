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

namespace SteliTour.Pages.Requests
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
        public Request Request { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id, string handler)
        {
            if (id == null || _context.Request == null)
            {
                return NotFound();
            }

            var request =  await _context.Request.FirstOrDefaultAsync(m => m.Id == id);
            if (request == null)
            {
                return NotFound();
            }
            Request = request;
            ViewData["UserId"] = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(handler != null && handler == "ChangeStatus")
            {
                await ChangeStatus(Request.Id);
                return RedirectToPage("./Index");
            }
            else if (handler != null && handler == "Delete")
            {
                await Delete(Request.Id);
                return RedirectToPage("./Index", new { type = "my" });
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.


        public async Task Delete(Guid id)
        {
            var req = await _context.Request.FirstOrDefaultAsync(x => x.Id == id);

            if (req != null)
            {
                _context.Request.Remove(req);

                var log = new log_19118105
                {
                    Id = Guid.NewGuid(),
                    Table = "Requests",
                    Operation = "Update",
                    Date = DateTime.Now,
                };

                await _context.log_19118105.AddAsync(log);

                await _context.SaveChangesAsync();
            }
        }
        public async Task ChangeStatus(Guid id)
        {
            var req = await _context.Request.FirstOrDefaultAsync(x => x.Id == id);

            if(req != null)
            {
                //A - означава answer/отговорено
                Request.Status = "A";
                Request.LastModified_19118105 = DateTime.Now;

                var log = new log_19118105
                {
                    Id = Guid.NewGuid(),
                    Table = "Requests",
                    Operation = "Update",
                    Date = DateTime.Now,
                };

                await _context.log_19118105.AddAsync(log);

                await _context.SaveChangesAsync();
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            Request.LastModified_19118105 = DateTime.Now;

            if (!ModelState.IsValid)
            {
                ViewData["UserId"] = User.FindFirstValue(ClaimTypes.NameIdentifier);
                return Page();
            }

            _context.Attach(Request).State = EntityState.Modified;

            var log = new log_19118105
            {
                Id = Guid.NewGuid(),
                Table = "Requests",
                Operation = "Update",
                Date = DateTime.Now,
            };

            await _context.log_19118105.AddAsync(log);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(Request.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool RequestExists(Guid id)
        {
          return (_context.Request?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

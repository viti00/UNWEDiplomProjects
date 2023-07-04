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

namespace SteliTour.Pages.Reservations
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly SteliTour.Data.ApplicationDbContext _context;

        public CreateModel(SteliTour.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(string id)
        {
            ViewData["DestinationId"] = Guid.Parse(id);
            ViewData["UserId"] = _context.Users.FirstOrDefault(x => x.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).Id;
            return Page();
        }

        [BindProperty]
        public Reservation Reservation { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            var destination = await _context.Destination.FirstOrDefaultAsync(x => x.Id == Reservation.DestinationId);

            Reservation.Id = Guid.NewGuid();
            Reservation.DateOfReservation = DateTime.Now;
            Reservation.LastModified_19118105 = DateTime.Now;
            Reservation.Payed = 0;
            Reservation.Remainign = ((Reservation.AdultCount * destination.AdultPrice) + (Reservation.KidsCount * destination.KidsPrice) + (Reservation.Under2YearsKidsCount * destination.Under2YearsKidsPrice));
            Reservation.Status = "A";


            if (!ModelState.IsValid || _context.Reservation == null || Reservation == null)
            {
                ViewData["DestinationId"] = Reservation.DestinationId;
                ViewData["UserId"] = _context.Users.FirstOrDefault(x => x.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).Id;
                return Page();
            }

            var log = new log_19118105
            {
                Id = Guid.NewGuid(),
                Table = "Reservations",
                Operation = "Insert",
                Date = DateTime.Now
            };

            await _context.log_19118105.AddAsync(log);
            await _context.Reservation.AddAsync(Reservation);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index", new { type = "my" });
        }

        public async Task<IActionResult> OnGetCheck(string id)
        {
            if (await _context.Reservation.FirstOrDefaultAsync(x => x.DestinationId == Guid.Parse(id) && x.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier)) != null)
            {
                return new JsonResult(true);
            }

            return new JsonResult(false);
        }
    }
}

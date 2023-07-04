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
    [IgnoreAntiforgeryToken(Order = 2000)]
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly SteliTour.Data.ApplicationDbContext _context;

        public EditModel(SteliTour.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Reservation Reservation { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id, string handler)
        {
            if (id == null || _context.Reservation == null)
            {
                return NotFound();
            }

            var reservation =  await _context.Reservation.Include(x=> x.Destination).FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }
            Reservation = reservation;

            ViewData["DestinationId"] = reservation.DestinationId;
            ViewData["UserId"] = _context.Users.FirstOrDefault(x => x.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).Id;

            if(handler != null && handler == "Delete")
            {
                await DenideReservation(reservation);
                return RedirectToPage("./Index");
            }
            else if(handler != null && handler == "Pay")
            {

            }

            return Page();
        }

        public async Task<IActionResult> OnGetChangeSum(Guid id, double sum)
        {
            var res = await _context.Reservation.FirstOrDefaultAsync(x => x.Id == id);

            res.Remainign -= sum;
            res.Payed += sum;

            var log = new log_19118105
            {
                Id = Guid.NewGuid(),
                Table = "Reservations",
                Operation = "Update",
                Date = DateTime.Now
            };

            await _context.log_19118105.AddAsync(log);

            await _context.SaveChangesAsync();

            return new JsonResult(res.Remainign);
        }

        public async Task DenideReservation(Reservation reservation)
        {

            var denided = new DenidedReservation()
            {
                Id = Guid.NewGuid(),
                UserEmail = reservation.User.Email,
                Destination = reservation.Destination.DisplayName,
                FirstName = reservation.FirstName,
                LastName = reservation.LastName,
                EGN = reservation.EGN,
                PhoneNumber = reservation.PhoneNumber,
                AdultCount = reservation.AdultCount,
                KidsCount = reservation.KidsCount,
                Under2YearsKidsCount = reservation.Under2YearsKidsCount,
                Payed = reservation.Payed,
                Remainign = reservation.Remainign,
                DateOfReservation = reservation.DateOfReservation.ToString(),
                DateOfCancelation = DateTime.Now,
                LastModified_19118105 = DateTime.Now
            };
            await _context.DenidedReservations.AddAsync(denided);
            var log_denided = new log_19118105
            {
                Id = Guid.NewGuid(),
                Table = "DenidedReservation",
                Operation = "Insert",
                Date = DateTime.Now
            };
            await _context.log_19118105.AddAsync(log_denided);

            _context.Reservation.Remove(reservation);

            var log = new log_19118105
            {
                Id = Guid.NewGuid(),
                Table = "Reservations",
                Operation = "Update",
                Date = DateTime.Now
            };
           
            await _context.log_19118105.AddAsync(log);
           
            await _context.SaveChangesAsync();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["DestinationId"] = Reservation.DestinationId;
                ViewData["UserId"] = _context.Users.FirstOrDefault(x => x.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).Id;
                return Page();
            }

            Reservation.LastModified_19118105 = DateTime.Now;
            _context.Attach(Reservation).State = EntityState.Modified;

            var log = new log_19118105
            {
                Id = Guid.NewGuid(),
                Table = "Reservations",
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
                if (!ReservationExists(Reservation.Id))
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

        private bool ReservationExists(Guid id)
        {
          return (_context.Reservation?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

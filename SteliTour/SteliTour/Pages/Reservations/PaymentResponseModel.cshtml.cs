using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SteliTour.Data;
using Stripe;

namespace SteliTour.Pages.Reservations
{
    [Authorize]
    public class PaymentResponseModelModel : PageModel
    {
        private readonly SteliTour.Data.ApplicationDbContext _context;

        public PaymentResponseModelModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync(string? payment_intent, string id)
        {
            if (string.IsNullOrEmpty(payment_intent))
            {
                RedirectToPage("Payment");
            }

            StripeConfiguration.ApiKey = "sk_test_51Mv2hWHyyNPNabXcXIw9jcUIU1S6o5xXc9JjcnEztrT6SBsEBynXb5HwH38cFX0ymqY8TYcvIEjgNjw3Z935IBzi00jLRDXsjB";
            var service = new PaymentIntentService();
            var paymentIntent = await service.GetAsync(payment_intent);
            if (paymentIntent.Status == "succeeded")
            {
                var resId = Guid.Parse(id);

                var res = await _context.Reservation.FirstOrDefaultAsync(x => x.Id == resId);

                if(res != null)
                {
                    res.Payed += paymentIntent.Amount;
                    res.Remainign -= paymentIntent.Amount;

                    var log = new log_19118105
                    {
                        Id = Guid.NewGuid(),
                        Table = "Reservations",
                        Operation = "Update",
                        Date = DateTime.Now
                    };

                    await _context.log_19118105.AddAsync(log);

                    await _context.SaveChangesAsync();
                };
            }
        }
    }
}

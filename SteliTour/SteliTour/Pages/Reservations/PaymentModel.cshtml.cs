using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SteliTour.Data;
using Stripe;

namespace SteliTour.Pages.Reservations
{
    [Authorize]
    public class PaymentModel : PageModel
    {
        private readonly SteliTour.Data.ApplicationDbContext _context;

        public PaymentModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string StripeToken { get; set; }

        public Guid Id { get; set; }
        public async Task OnGetAsync(Guid id, long amount)
        {
            var res = await _context.Reservation.FirstOrDefaultAsync(x => x.Id == id);
            if (res.Remainign <= 0)
            {
                RedirectToPage("/");
            }

            Id = res.Id;

            StripeConfiguration.ApiKey = "sk_test_51Mv2hWHyyNPNabXcXIw9jcUIU1S6o5xXc9JjcnEztrT6SBsEBynXb5HwH38cFX0ymqY8TYcvIEjgNjw3Z935IBzi00jLRDXsjB";
            var options = new PaymentIntentCreateOptions
            {
                Amount = amount,
                Currency = "bgn",
                Description = "Превод на пари за резервация#" + res.Id,
                AutomaticPaymentMethods = new
           PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                },
            };
            var service = new PaymentIntentService();
            var paymentIntent = service.Create(options);
            this.StripeToken = paymentIntent.ClientSecret;
        }
    }
}

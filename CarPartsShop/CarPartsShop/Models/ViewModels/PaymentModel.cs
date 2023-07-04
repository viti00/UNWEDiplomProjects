using Microsoft.AspNetCore.Mvc;

namespace CarPartsShop.Models.ViewModels
{
    public class PaymentModel
    {
        [BindProperty]
        public string StripeToken { get; set; }
    }
}

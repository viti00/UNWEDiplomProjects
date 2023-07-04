using CarPartsShop.Data;
using CarPartsShop.Infrastructure;
using CarPartsShop.Services.CartService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CarPartsShop.Controllers
{
    public class CartsController : Controller
    {
        private readonly ICartService cartService;

        public CartsController(ICartService cartService)
        {
            this.cartService = cartService;
        }

        [HttpPost]
        [Authorize]
        public bool AddToCart(int count, string partId)
        {
            bool isOkay = false;
            var userId = User.GetId();

            var cart = cartService.GetCartByUserId(userId);

            if (cart == null)
            {
                cartService.CreateNewCart(userId);
                cart = cartService.GetCartByUserId(userId);
            }

            if(cart.Parts.Any(x=> x.PartId == partId))
            {
                var part = cart.Parts.FirstOrDefault(x => x.PartId == partId);

              isOkay = cartService.AddCountIfPartExist(part, count, cart);
            }
            else
            {
               isOkay = cartService.AddPartToCart(partId, count, cart.Id);
            }

            return isOkay;
        }

        [Authorize]
        public double GetNewTotalPrice()
        {
            var cart = cartService.GetAllCartInfo(User.GetId());

            var totalPrice = cartService.GetTotalPrice(cart);

            return totalPrice;
        }

        [HttpPost]
        [Authorize]
        public bool EditPartCount(string partId, int newCount)
        {
            var cart = cartService.GetCartByUserId(User.GetId());

           var isOkay = cartService.CartEdit(cart, partId, newCount);

            return isOkay;
        }

        [Authorize]
        public void RemovePartFromCart(string partId)
        {
            var cart = cartService.GetCartByUserId(User.GetId());

            cartService.RemovePart(cart, partId);
        }

        [Authorize]
        public IActionResult Details()
        {
            var userId = User.GetId();
            var cart = cartService.GetAllCartInfo(userId);

            return View(cart);
        }


        public int GetCartPartsCount()
        {
            var userId = User.GetId();

            if(userId == null)
            {
                return 0;
            }

            var count = cartService.GetPartsCount(userId);

            return count;
        }
    }
}

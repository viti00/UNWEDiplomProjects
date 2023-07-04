using CarPartsShop.Data.Models;

namespace CarPartsShop.Services.CartService
{
    public interface ICartService
    {
        public bool CartEdit(Cart cart, string partId, int newCount);

        public void DeleteCart(string cartId, string userId);

        public bool AddPartToCart(string partId, int count, string cartId);

        public void CreateNewCart(string userId);

        public void CloseCart(string cartId, string userId);

        public Cart GetCartByUserId(string userId);

        public Cart GetAllCartInfo(string userId);

        public double GetTotalPrice(Cart cart);

        public void RemovePart(Cart cart, string partId);

        public bool AddCountIfPartExist(PickedUpPart part, int count, Cart cart);

        public Cart GetCurrentCart(string cartId, string userId);

        public int GetPartsCount(string userId);
    }
}

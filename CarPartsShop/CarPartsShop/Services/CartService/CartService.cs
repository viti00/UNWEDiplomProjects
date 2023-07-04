using CarPartsShop.Data;
using CarPartsShop.Data.Models;
using CarPartsShop.Models.FormModels;
using CarPartsShop.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CarPartsShop.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly CarPartsShopDbContext context;
        public CartService(CarPartsShopDbContext context)
        {
            this.context = context;
        }

        public bool AddPartToCart(string partId, int count, string cartId)
        {
            var part = context.Parts.FirstOrDefault(x => x.Id == partId);

            if (part.StockQnt < count)
            {
                return false;
            }
            var pickedUpPart = new PickedUpPart
            {
                CartId = cartId,
                PartId = partId,
                Count = count,
                LastModified_19118067 = DateTime.Now
            };

            var log_picked = new log_19118067
            {
                TableName = "PickedUpPart",
                OperationType = "Insert",
                OperationCreateTime = DateTime.Now
            };
            context.log_19118067.Add(log_picked);
            var log_cart = new log_19118067
            {
                TableName = "Carts",
                OperationType = "Update",
                OperationCreateTime = DateTime.Now
            };
            context.log_19118067.Add(log_cart);
            context.PickedUpParts.Add(pickedUpPart);
            context.SaveChanges();


            return true;
        }

        public bool CartEdit(Cart cart, string partId, int newCount)
        {
            var partInfo = context
                            .PickedUpParts
                            .FirstOrDefault(x => x.CartId == cart.Id && x.PartId == partId);

            if (partInfo.Count + newCount > context.Parts.FirstOrDefault(x=> x.Id == partId).StockQnt) {
                return false;
            }

            var log_picked = new log_19118067
            {
                TableName = "PickedUpPart",
                OperationType = "Update",
                OperationCreateTime = DateTime.Now
            };
            context.log_19118067.Add(log_picked);
            var log_cart = new log_19118067
            {
                TableName = "Carts",
                OperationType = "Update",
                OperationCreateTime = DateTime.Now
            };
            context.log_19118067.Add(log_cart);

            partInfo.Count = newCount;
            cart.LastModified_19118067 = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        public void CloseCart(string cartId, string userId)
        {
            var cart = GetCurrentCart(cartId, userId);

            cart.CartStatusId = context.CartStatuses.FirstOrDefault(x => x.Status == "Closed").Id;
            cart.LastModified_19118067 = DateTime.Now;

            var log = new log_19118067
            {
                TableName = "Carts",
                OperationType = "Update",
                OperationCreateTime = DateTime.Now
            };
            context.log_19118067.Add(log);
        }

        public void CreateNewCart(string userId)
        {
            var cart = new Cart
            {
                UserId = userId,
                CartStatusId = context.CartStatuses.FirstOrDefault(x => x.Status == "Active").Id,
                LastModified_19118067 = DateTime.Now
            };
            var log = new log_19118067
            {
                TableName = "Carts",
                OperationType = "Insert",
                OperationCreateTime = DateTime.Now
            };
            context.log_19118067.Add(log);
            context.Carts.Add(cart);
            context.SaveChanges();
        }

        public void DeleteCart(string cartId, string userId)
        {
            var cart = context.Carts.FirstOrDefault(x => x.Id == cartId && x.UserId == userId);

            if (cart == null)
            {
                return;
            }

            var pickedParts = context.PickedUpParts.Where(x => x.CartId == cart.Id).ToList();

            var log_picked = new log_19118067
            {
                TableName = "PickedUpPart",
                OperationType = "Delete",
                OperationCreateTime = DateTime.Now
            };
            context.log_19118067.Add(log_picked);

            var log_carts = new log_19118067
            {
                TableName = "Carts",
                OperationType = "Delete",
                OperationCreateTime = DateTime.Now
            };
            context.log_19118067.Add(log_carts);

            context.PickedUpParts.RemoveRange(pickedParts);
            context.Carts.Remove(cart);
            context.SaveChanges();
        }

        public double GetTotalPrice(Cart cart)
        {
            double totalPrice = 0;

            foreach (var part in cart.Parts)
            {
                totalPrice += part.Count * part.Part.PartPrice;
            }

            return totalPrice;
        }

        public void RemovePart(Cart cart, string partId)
        {
            var pickedPart = context.PickedUpParts.FirstOrDefault(x => x.CartId == cart.Id && x.PartId == partId);
            var log_picked = new log_19118067
            {
                TableName = "PickedUpPart",
                OperationType = "Delete",
                OperationCreateTime = DateTime.Now
            };
            context.log_19118067.Add(log_picked);
            var log_cart = new log_19118067
            {
                TableName = "Carts",
                OperationType = "Update",
                OperationCreateTime = DateTime.Now
            };
            context.log_19118067.Add(log_cart);
            context.PickedUpParts.Remove(pickedPart);
            cart.LastModified_19118067 = DateTime.Now;
            context.SaveChanges(true);
        }

        public bool AddCountIfPartExist(PickedUpPart part, int count, Cart cart)
        {

            if (part.Part.StockQnt < count)
            {
                return false;
            }
            part.Count += count;
            cart.LastModified_19118067 = DateTime.Now;

            var log_picked = new log_19118067
            {
                TableName = "PickedUpPart",
                OperationType = "Update",
                OperationCreateTime = DateTime.Now
            };
            context.log_19118067.Add(log_picked);

            var log_carts = new log_19118067
            {
                TableName = "Carts",
                OperationType = "Update",
                OperationCreateTime = DateTime.Now
            };
            context.log_19118067.Add(log_carts);
            context.SaveChanges();


            return true;

        }

        public Cart GetAllCartInfo(string userId)
            => context.Carts
                      .Include(x => x.CartStatus)
                      .Include(x => x.Parts)
                            .ThenInclude(x => x.Part)
                            .ThenInclude(x => x.PartCondtion)
                      .Include(x => x.Parts)
                            .ThenInclude(x => x.Part)
                            .ThenInclude(x => x.PartImages)
                      .Include(x => x.Parts)
                            .ThenInclude(x => x.Part)
                            .ThenInclude(x => x.PartType)
                      .FirstOrDefault(x => x.UserId == userId && x.CartStatus.Status == "Active");
        public Cart GetCartByUserId(string userId)
            => context.Carts
                    .Include(x => x.CartStatus)
                    .Include(x => x.Parts)
                        .ThenInclude(x => x.Part.PartImages)
                    .FirstOrDefault(x => x.UserId == userId && x.CartStatus.Status == "Active");

        public Cart GetCurrentCart(string cartId, string userId)
            => context
                    .Carts
                    .Include(x => x.Parts)
                        .ThenInclude(x => x.Part.PartImages)
                    .FirstOrDefault(x => x.Id == cartId && x.UserId == userId);

        public int GetPartsCount(string userId)
        {
            var cart = GetCartByUserId(userId);

            if (cart == null)
            {
                return 0;
            }

            return cart.Parts.Count;
        }
    }
}

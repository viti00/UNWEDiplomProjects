using CarPartsShop.Data.Models;
using CarPartsShop.Models.FormModels;
using CarPartsShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace CarPartsShop.Services.OrderService
{
    public interface IOrderService
    {
        public string OrderMake(string userId, OrderInformationForm orderInfo);

        public void ValidateProductsCount(OrderInformationForm model, ModelStateDictionary ms);

        public CartOrderModel FillCartInfo(CartOrderModel cart, string userId);

        public QueryModelOrders GetOrders(QueryModelOrders query, string userId);

        public Order GetOrderById(string id);

        public bool RejectOrder(string id);

        public void StatusSent(string id);
        public void StatusDelivered(string id);

        public List<OrderViewModel> GetOrdersViewModel();

        public Task SetOrderToPaid(string orderId);

        public void SaveChanges(string orderId, string userId);

        public void RemoveOrder(string orderId);
    }
}

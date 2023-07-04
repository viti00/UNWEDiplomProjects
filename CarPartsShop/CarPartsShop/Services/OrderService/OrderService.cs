using CarPartsShop.Data;
using CarPartsShop.Data.Models;
using CarPartsShop.Models.FormModels;
using CarPartsShop.Models.ViewModels;
using CarPartsShop.Services.CartService;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace CarPartsShop.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly CarPartsShopDbContext context;
        private readonly ICartService cartService;

        public OrderService(CarPartsShopDbContext context, ICartService cartService)
        {
            this.context = context;
            this.cartService = cartService;
        }

        public CartOrderModel FillCartInfo(CartOrderModel cart, string userId)
        {
           var currCart = cartService.GetCartByUserId(userId);

            if(currCart == null)
            {
                return null;
            }
            cart = new CartOrderModel
            {
                Id = currCart.Id,
                Parts = currCart.Parts.ToList()
            };

            return cart;
        }

        public QueryModelOrders GetOrders(QueryModelOrders query,string userId)
        {
            List<OrderViewModel> orders = new List<OrderViewModel>();

            if(userId != null)
            {
                orders = context
                        .Orders
                        .Where(x => x.UserId == userId)
                        .Select(x => new OrderViewModel
                        {
                            Id = x.Id,
                            FirstName = x.FirstName,
                            LastName = x.LastName,
                            PhoneNumber = x.PhoneNumber,
                            Address = x.Address,
                            Email = x.User.Email,
                            Status = x.OrderStatus.Status,
                            LastModified_19118067 = x.LastModified_19118067,
                            IsPaid = x.IsPaid == true?"Да":"Не"
                        })
                        .ToList();
            }
            else
            {
                orders = GetOrdersViewModel();
            }

            if(query.Search != null)
            {
                orders = orders.Where(x => x.Status.Contains(query.Search, StringComparison.OrdinalIgnoreCase)
                                        || x.Address.Contains(query.Search, StringComparison.OrdinalIgnoreCase)
                                        || x.FirstName.Contains(query.Search, StringComparison.OrdinalIgnoreCase)
                                        || x.LastName.Contains(query.Search, StringComparison.OrdinalIgnoreCase)
                                        || x.Email.Contains(query.Search, StringComparison.OrdinalIgnoreCase)
                                        || x.PhoneNumber.Contains(query.Search, StringComparison.OrdinalIgnoreCase)
                                        ).ToList();
            }

            orders = query.SortOrders switch
            {
                SortOrders.Default => orders.OrderByDescending(x => x.LastModified_19118067).ToList(),
                SortOrders.DateAscending => orders.OrderBy(x => x.LastModified_19118067).ToList(),
                SortOrders.DateDescending => orders.OrderByDescending(x => x.LastModified_19118067).ToList(),
                _=> orders.OrderByDescending(x => x.LastModified_19118067).ToList()
            };

            orders = query.FilterOrders switch
            {
                FilterOrders.None => orders.ToList(),
                FilterOrders.AwaitingApprolval => orders.Where(x=> x.Status == "Обработва се").ToList(),
                FilterOrders.Delivering => orders.Where(x => x.Status == "Предадена на куриер").ToList(),
                FilterOrders.Delivered => orders.Where(x => x.Status == "Доставена").ToList(),
                FilterOrders.Denided => orders.Where(x => x.Status == "Отказана").ToList(),
                _=> orders.ToList()
            };

            var queryModel = new QueryModelOrders
            {
                Orders = orders,
                FilterOrders = query.FilterOrders,
                SortOrders = query.SortOrders,
                Search = query.Search
            };

            return queryModel;
        }

        public Order GetOrderById(string id)
        {
            var order = context.Orders
                .Include(p => p.Parts)
                    .ThenInclude(x => x.Part.PartImages)
                .Include(p => p.Parts)
                    .ThenInclude(x => x.Part.PartCondtion)
                .Include(s=> s.OrderStatus)
                .FirstOrDefault(x => x.Id == id);

            return order;
        }

        public string OrderMake(string userId, OrderInformationForm orderInfo)
        {
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                FirstName = orderInfo.FirstName,
                LastName = orderInfo.LastName,
                PhoneNumber = orderInfo.Phone,
                City = orderInfo.City,
                PostCode = orderInfo.PostCode,
                Address = orderInfo.Address,
                Parts = orderInfo.Cart.Parts.ToList(),
                LastModified_19118067 = DateTime.Now,
                OrderStatus = context.OrderStatuses.FirstOrDefault(x => x.Status == "Обработва се"),
                IsPaid = orderInfo.IsPaid
            };

            var log = new log_19118067
            {
                TableName = "Orders",
                OperationType = "Insert",
                OperationCreateTime = DateTime.Now
            };

            //cartService.CloseCart(orderInfo.Cart.Id, userId);
            context.Orders.Add(order);
            context.log_19118067.Add(log);
            context.SaveChanges();
            return order.Id;
        }

        public void ValidateProductsCount(OrderInformationForm model, ModelStateDictionary ms)
        {

            foreach (var part in model.Cart.Parts)
            {
                if(part.Part.StockQnt - part.Count < 0)
                {
                    ms.AddModelError($"{part.PartId}", $"Имаме {part.Part.StockQnt} налични бройки! Вие искате да купите {part.Count}");
                }
                else
                {
                    var log = new log_19118067
                    {
                        TableName = "Parts",
                        OperationType = "Update",
                        OperationCreateTime = DateTime.Now
                    };
                    context.log_19118067.Add(log);
                    part.Part.StockQnt -= part.Count;
                    part.LastModified_19118067 = DateTime.Now;
                }
            }
        }

        public void RemoveOrder(string orderId)
        {
            var order = GetOrderById(orderId);
            context.Orders.Remove(order);
            context.SaveChanges();
        }

        public void SaveChanges(string orderId, string userId)
        {
            var log = new log_19118067
            {
                TableName = "Orders",
                OperationType = "Update",
                OperationCreateTime = DateTime.Now
            };

            var order = GetOrderById(orderId);
            order.IsPaid = true;

            cartService.CloseCart(cartService.GetCartByUserId(userId).Id, userId);
            context.log_19118067.Add(log);
            context.SaveChanges();
        }

        public bool RejectOrder(string id)
        {
            var order = GetOrderById(id);

            if(order.OrderStatus.Status !="Обработва се")
            {
                return false;
            }
            if (order == null)
            {
                return false;
            }

            foreach (var part in order.Parts)
            {
                var p = context.Parts.FirstOrDefault(x => x.Id == part.PartId);
                if (p != null)
                {
                    p.StockQnt += part.Count;
                    p.LastModified_19118067 = DateTime.Now;
                }
            }

            var log = new log_19118067
            {
                TableName = "Orders",
                OperationType = "Update",
                OperationCreateTime = DateTime.Now
            };
            context.log_19118067.Add(log);

            order.OrderStatus = context.OrderStatuses.FirstOrDefault(x => x.Status == "Отказана");
            context.SaveChanges();
            return true;
        }

        public void StatusSent(string id)
        {
            var order = GetOrderById(id);

            if (order != null)
            {
                order.OrderStatusId = context.OrderStatuses.FirstOrDefault(x=> x.Status == "Предадена на куриер").Id;
                order.LastModified_19118067 = DateTime.Now;
                var log = new log_19118067
                {
                    TableName = "Orders",
                    OperationType = "Update",
                    OperationCreateTime = DateTime.Now
                };
                context.log_19118067.Add(log);
                context.SaveChanges();
            }
        }

        public void StatusDelivered(string id)
        {
            var order = GetOrderById(id);

            if (order != null)
            {
                order.OrderStatusId = context.OrderStatuses.FirstOrDefault(x => x.Status == "Доставена").Id;
                order.LastModified_19118067 = DateTime.Now;
                var log = new log_19118067
                {
                    TableName = "Orders",
                    OperationType = "Update",
                    OperationCreateTime = DateTime.Now
                };
                context.log_19118067.Add(log);
                context.SaveChanges();
            }
        }

        public List<OrderViewModel> GetOrdersViewModel()
        {
            var orders = context
                        .Orders
                        .Select(x => new OrderViewModel
                        {
                            Id = x.Id,
                            FirstName = x.FirstName,
                            LastName = x.LastName,
                            PhoneNumber = x.PhoneNumber,
                            Address = x.Address,
                            Email = x.User.Email,
                            Status = x.OrderStatus.Status,
                            LastModified_19118067 = x.LastModified_19118067
                        }).ToList();

            return orders;
        }

        public async Task SetOrderToPaid(string orderId)
        {
            var order = GetOrderById(orderId);
            if (order != null)
            {
                order.IsPaid = true;
                await context.SaveChangesAsync();
            }
        }
    }
}

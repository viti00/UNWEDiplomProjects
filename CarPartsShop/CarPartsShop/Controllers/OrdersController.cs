using CarPartsShop.Data;
using CarPartsShop.Data.Models;
using CarPartsShop.Infrastructure;
using CarPartsShop.Models.FormModels;
using CarPartsShop.Models.ViewModels;
using CarPartsShop.Services.CartService;
using CarPartsShop.Services.OrderService;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Stripe;
using static CarPartsShop.Infrastructure.RolesConstants;
using static CarPartsShop.Models.OrderVariable;


namespace CarPartsShop.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [Authorize]
        public IActionResult OrderInfo(OrderInformationForm model)
        {
            model.Cart = orderService.FillCartInfo(model.Cart, User.GetId());

            ModelState.Clear();

            if (model.Cart == null)
            {
                return RedirectToAction("Index", "Parts");
            }

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public IActionResult MakeOrder(OrderInformationForm model)
        {

            model.Cart = orderService.FillCartInfo(model.Cart, User.GetId());

            orderService.ValidateProductsCount(model, ModelState);
            if (!ModelState.IsValid)
            {
                return View("OrderInfo", model);
            }

            OrderId = orderService.OrderMake(User.GetId(), model);

            if (model.PayNow)
            {
                return Redirect($"/Orders/PaymentPage");
            }

            return Redirect($"/Orders/All/{User.GetId()}");
        }

        [Authorize]
        public IActionResult All([FromQuery] QueryModelOrders query, string userId)
        {
            var orders = orderService.GetOrders(query, userId);

            return View(orders);
        }

        [Authorize]
        public IActionResult OrderDetails(string id)
        {
            var order = orderService.GetOrderById(id);
            return View(order);
        }

        [HttpPost]
        [Authorize]
        public bool RejectOrder(string id)
        {
            var result = orderService.RejectOrder(id);

            return result;
        }

        [HttpGet]
        [Authorize(Roles = nameof(Administrator))]
        public IActionResult ChangeStatusToSent(string id)
        {
            orderService.StatusSent(id);
            return RedirectToAction("All", "Orders");
        }

        [HttpGet]
        [Authorize(Roles = nameof(Administrator))]
        public IActionResult ChangeStatusToDelivered(string id)
        {
            orderService.StatusDelivered(id);
            return RedirectToAction("All", "Orders");
        }

        public IActionResult ExportToExcel(string search, SortOrders sortOrders, FilterOrders filterOrders)
        {
            var orders = orderService.GetOrdersViewModel();

            var query = new QueryModelOrders
            {
                Orders = orders,
                Search = search,
                FilterOrders = filterOrders,
                SortOrders = sortOrders
            };

            var toExport = orderService.GetOrders(query, null);

            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Поръчки");

            worksheet.Cell(1, 1).Value = "Поръчка №";
            worksheet.Cell(1, 2).Value = "Име";
            worksheet.Cell(1, 3).Value = "Имейл";
            worksheet.Cell(1, 4).Value = "Телефон";
            worksheet.Cell(1, 5).Value = "Адрес за доставка";
            worksheet.Cell(1, 6).Value = "Статус";
            worksheet.Cell(1, 7).Value = "Дата";

            for (int i = 0; i < toExport.Orders.Count; i++)
            {
                var order = toExport.Orders[i];
                worksheet.Cell(i + 2, 1).Value = order.Id;
                worksheet.Cell(i + 2, 2).Value = order.FirstName + " " + order.LastName;
                worksheet.Cell(i + 2, 3).Value = order.Email;
                worksheet.Cell(i + 2, 4).Value = order.PhoneNumber;
                worksheet.Cell(i + 2, 5).Value = order.Address;
                worksheet.Cell(i + 2, 6).Value = order.Status;
                worksheet.Cell(i + 2, 7).Value = order.LastModified_19118067;
            }

            var fileName = "orders.xlsx";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileName);
            workbook.SaveAs(filePath);

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                stream.CopyTo(memory);
            }
            memory.Position = 0;

            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        
        public IActionResult PaymentPage()
        {
            var order = orderService.GetOrderById(OrderId);

            if(order == null)
            {
                return Redirect($"/Orders/All/{User.GetId()}");
            }

            if (order.IsPaid)
            {
                return Redirect($"/Orders/All/{User.GetId()}");
            };
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            StripeConfiguration.ApiKey = config.GetSection("APIS:StripNetSecretKey:Key").Value;

            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(order.Parts.Sum(x => x.Part.PartPrice *
               x.Count)),
                Currency = "bgn",
                Description = "Поръчка на авточасти #" + order.Id,
                AutomaticPaymentMethods = new
                PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                },
            };
            var service = new PaymentIntentService();
            var paymentIntent = service.Create(options);
            var payModel = new PaymentModel();
            payModel.StripeToken = paymentIntent.ClientSecret;
            return View(payModel);
        }

        public IActionResult PaymentResponsePage(string payment_intent)
        {
            if (string.IsNullOrEmpty(payment_intent))
            {
               return Redirect($"/Orders/PaymentPage/{OrderId}");
            }
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            StripeConfiguration.ApiKey = config.GetSection("APIS:StripNetSecretKey:Key").Value;
            var service = new PaymentIntentService();
            var paymentIntent =  service.Get(payment_intent);
            if (paymentIntent.Status == "succeeded")
            {
                orderService.SaveChanges(OrderId, User.GetId());
            }
            else
            {
                orderService.RemoveOrder(OrderId);
               return Redirect($"/Orders/PaymentPage/{OrderId}");
            }
            return View();
        }
    }
}

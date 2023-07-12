using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FoodAdditivesShop.Data;

namespace FoodAdditivesShop.Pages.Orders
{
    [Authorize(Roles ="Admin, OrdersManager")]
    public class ExportModel : PageModel
    {
        private readonly FoodAdditivesShop.Data.ApplicationDbContext _context;

        public ExportModel(FoodAdditivesShop.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Order> Order { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Orders != null)
            {
                Order = await _context.Orders
                .Include(o => o.User).ToListAsync();
            }
        }

        public async Task<IActionResult> OnGetExport()
        {
            if (_context.Orders != null)
            {
                Order = await _context.Orders
                .Include(o => o.User).ToListAsync();

                var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Поръчки");

                worksheet.Cell(1, 1).Value = "Поръчка №";
                worksheet.Cell(1, 2).Value = "Име";
                worksheet.Cell(1, 3).Value = "Телефон";
                worksheet.Cell(1, 4).Value = "Адрес за доставка";
                worksheet.Cell(1, 5).Value = "Количка №";
                worksheet.Cell(1, 6).Value = "Продукти";
                worksheet.Cell(1, 7).Value = "Обща сума";
                worksheet.Cell(1, 8).Value = "Обща сума";
                worksheet.Cell(1, 9).Value = "Дата";

                for (int i = 0; i < Order.Count; i++)
                {
                    var order = Order[i];
                    var cart = await _context
                        .Carts
                        .Include(x => x.CartProducts)
                        .ThenInclude(x => x.Product)
                        .FirstOrDefaultAsync(x => x.Id == order.CartId);

                    worksheet.Cell(i + 2, 1).Value = order.Id.ToString();
                    worksheet.Cell(i + 2, 2).Value = order.FName + " " + order.LName;
                    worksheet.Cell(i + 2, 3).Value = order.Phone;
                    worksheet.Cell(i + 2, 4).Value = order.Addres;
                    worksheet.Cell(i + 2, 5).Value = order.CartId.ToString();
                    worksheet.Cell(i + 2, 6).Value = string.Join("\n", GetProducts(cart.CartProducts));
                    worksheet.Cell(i + 2, 7).Value = cart.CartProducts.Sum(x=> x.ProductCount * x.Product.ProductPrice) + "лв.";
                    worksheet.Cell(i + 2, 8).Value = order.Status;
                    worksheet.Cell(i + 2, 9).Value = order.LastModified_19118119;
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

            return BadRequest();
        }

        private string GetProducts(List<CartProduct> products)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var product in products)
            {

                sb.AppendLine($"{product.Product.ProductName} - Ед. Цена: {product.Product.ProductPrice} лв.: {product.ProductCount} броя");
            }

            return sb.ToString();
        }
    }
}

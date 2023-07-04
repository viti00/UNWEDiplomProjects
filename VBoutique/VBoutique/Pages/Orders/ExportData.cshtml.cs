using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VBoutique.Data;

namespace VBoutique.Pages.Orders
{
    public class ExportDataModel : PageModel
    {
        private readonly VBoutique.Data.ApplicationDbContext _context;

        public ExportDataModel(VBoutique.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Order> Order { get;set; } = default!;

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
                worksheet.Cell(1, 8).Value = "Дата";

                for (int i = 0; i < Order.Count; i++)
                {
                    var order = Order[i];
                    var cart = await _context
                        .ShoppingCarts
                        .Include(x => x.ShoppingCartItems)
                        .ThenInclude(x => x.Product)
                        .FirstOrDefaultAsync(x => x.Id == order.ShoppingCartId);

                    worksheet.Cell(i + 2, 1).Value = order.Id.ToString();
                    worksheet.Cell(i + 2, 2).Value = order.FirstName + " " + order.LastName;
                    worksheet.Cell(i + 2, 3).Value = order.PhoneNumber;
                    worksheet.Cell(i + 2, 4).Value = order.DeliveryAddress;
                    worksheet.Cell(i + 2, 5).Value = order.ShoppingCartId.ToString();
                    worksheet.Cell(i + 2, 6).Value = GetProducts(cart.ShoppingCartItems);
                    worksheet.Cell(i + 2, 7).Value = CalcolateTotalAmount(cart.ShoppingCartItems) + " лв.";
                    worksheet.Cell(i + 2, 8).Value = order.LastModified_19118155;
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
        private double CalcolateTotalAmount(List<ShoppingCartItems> shoppingCartItems)
        {
            double total = 0;

            foreach (var item in shoppingCartItems)
            {
                if (item.Product is Bag)
                {
                    total += item.Product.Price;
                }
                else if (item.Product is Shoe)
                {
                    total += (item.Size35Count + item.Size36Count + item.Size37Count
                          + item.Size38Count + item.Size39Count + item.Size40Count + item.Size41Count) * item.Product.Price;
                };
            }

            return total;
        }

        private string GetProducts(List<ShoppingCartItems> shoppingCartItems)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in shoppingCartItems)
            {
                if (item.Product is Bag bag)
                {
                    sb.AppendLine($"{bag.Make} - {bag.Model}: {item.ProductCount} броя");
                }
                else if (item.Product is Shoe shoe)
                {
                    if(item.Size35Count > 0)
                    {
                        sb.AppendLine($"№35: {item.Size35Count} броя");
                    }
                    if (item.Size36Count > 0)
                    {
                        sb.AppendLine($"№36: {item.Size36Count} броя");
                    }
                    if (item.Size37Count > 0)
                    {
                        sb.AppendLine($"№37: {item.Size37Count} броя");
                    }
                    if (item.Size38Count > 0)
                    {
                        sb.AppendLine($"№38: {item.Size38Count} броя");
                    }
                    if (item.Size39Count > 0)
                    {
                        sb.AppendLine($"№39: {item.Size39Count} броя");
                    }
                    if (item.Size40Count > 0)
                    {
                        sb.AppendLine($"№40: {item.Size40Count} броя");
                    }
                    if (item.Size41Count > 0)
                    {
                        sb.AppendLine($"№41: {item.Size41Count} броя");
                    }
                };

            }

            return sb.ToString();
        }
    }
}

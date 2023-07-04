using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HomeEssentials.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using static NuGet.Packaging.PackagingConstants;
using ClosedXML.Excel;

namespace HomeEssentials.Pages.Orders
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly HomeEssentials.Data.ApplicationDbContext _context;

        public IndexModel(HomeEssentials.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Order> Order { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Orders != null)
            {
                if (User.IsInRole("Administrator") || User.IsInRole("OrdersManager"))
                {
                    Order = await _context.Orders
                    .Include(o => o.User)
                    .Include(x => x.Status)
                    .ToListAsync();
                }
                else
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    Order = await _context.Orders
                    .Include(o => o.User)
                    .Include(x => x.Status)
                    .Where(x => x.UserId == userId)
                    .ToListAsync();
                }
            }
        }

        public async Task<IActionResult> OnGetDownload()
        {
            Order = await _context.Orders
              .Include(o => o.User)
              .Include(x => x.Status)
              .ToListAsync();

            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Поръчки");

            worksheet.Cell(1, 1).Value = "Поръчка №";
            worksheet.Cell(1, 2).Value = "Име";
            worksheet.Cell(1, 3).Value = "Телефон";
            worksheet.Cell(1, 4).Value = "Адрес за доставка";
            worksheet.Cell(1, 5).Value = "Количка №";
            worksheet.Cell(1, 6).Value = "Обща сума";
            worksheet.Cell(1, 7).Value = "Статус";
            worksheet.Cell(1, 8).Value = "Дата";

            for (int i = 0; i < Order.Count; i++)
            {
                var order = Order[i];
                var cart = await _context
                    .Carts
                    .Include(x => x.CartItems)
                    .ThenInclude(x => x.Item)
                    .FirstOrDefaultAsync(x => x.Id == order.CartId);

                worksheet.Cell(i + 2, 1).Value = order.Id.ToString();
                worksheet.Cell(i + 2, 2).Value = order.FirstName + " " + order.LastName;
                worksheet.Cell(i + 2, 3).Value = order.Phone;
                worksheet.Cell(i + 2, 4).Value = order.Address;
                worksheet.Cell(i + 2, 5).Value = order.CartId.ToString();
                worksheet.Cell(i + 2, 6).Value = cart.CartItems.Sum(x => x.ItemCount * x.Item.Price) + " лв.";
                worksheet.Cell(i + 2, 7).Value = order.Status.Name;
                worksheet.Cell(i + 2, 8).Value = order.CreatedOn;
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
    }
}

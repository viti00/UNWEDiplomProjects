using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FishermanMarket.Data;
using FishermanMarket.Data.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using ClosedXML.Excel;

namespace FishermanMarket.Pages.Orders
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly FishermanMarket.Data.ApplicationDbContext _context;

        public IndexModel(FishermanMarket.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Order> Order { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Orders != null)
            {


                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (User.IsInRole("Admin") || User.IsInRole("OrdersSender"))
                {
                    Order = await _context.Orders
                .Include(o => o.User)
                .ToListAsync();
                }
                else
                {
                    Order = await _context.Orders
                .Include(o => o.User).Where(x => x.UserId == userId)
                .ToListAsync();
                }
            }
        }

        public async Task OnPostChangeStatus(Guid id)
        {
            var order = await _context.Orders
               .Include(o => o.User)
               .FirstOrDefaultAsync(x => x.Id == id);

            order.Status = "Изпратена";
            order.LastModified_19118062 = DateTime.Now;

            

            var order_log = new log_19118062
            {
                Id = Guid.NewGuid(),
                Table = "Orders",
                Operation = "Update",
                Time = DateTime.Now
            };

            await _context.log_19118062.AddAsync(order_log);
            await _context.SaveChangesAsync();

            Order = await _context.Orders
                .Include(o => o.User)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostReport()
        {
            var orders = await _context.Orders.ToListAsync();

            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Поръчки");

            worksheet.Cell(1, 1).Value = "Поръчка №";
            worksheet.Cell(1, 2).Value = "Име";
            worksheet.Cell(1, 3).Value = "Телефон";
            worksheet.Cell(1, 4).Value = "Адрес за доставка";
            worksheet.Cell(1, 5).Value = "Статус";
            worksheet.Cell(1, 6).Value = "Дата";

            for (int i = 0; i < orders.Count; i++)
            {
                var order = orders[i];
                worksheet.Cell(i + 2, 1).Value = order.Id.ToString();
                worksheet.Cell(i + 2, 2).Value = order.FirstName + " " + order.LastName;
                worksheet.Cell(i + 2, 3).Value = order.PhoneNumber;
                worksheet.Cell(i + 2, 4).Value = order.DeliveryAddres;
                worksheet.Cell(i + 2, 5).Value = order.Status;
                worksheet.Cell(i + 2, 6).Value = order.OrderDate;
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
        public async Task OnPostDelete(Guid id)
        {
            var order = await _context.Orders
               .Include(o => o.User)
               .FirstOrDefaultAsync(x => x.Id == id);

            var products = _context.BagProducts.Include(x => x.Bag).Where(x => x.BagId == order.BagId).ToList();

            var bagProducts_log = new log_19118062
            {
                Id = Guid.NewGuid(),
                Table = "BagProducts",
                Operation = "Update",
                Time = DateTime.Now
            };

            var order_log = new log_19118062
            {
                Id = Guid.NewGuid(),
                Table = "Orders",
                Operation = "Update",
                Time = DateTime.Now
            };


            _context.BagProducts.RemoveRange(products);
            _context.Orders.Remove(order);

            await _context.log_19118062.AddAsync(bagProducts_log);
            await _context.log_19118062.AddAsync(order_log);

            await _context.SaveChangesAsync();

            Order = await _context.Orders
                .Include(o => o.User)
                .ToListAsync();
        }
    }
}

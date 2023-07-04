using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RestaurantOrders.Data;
using System.Text;

namespace RestaurantOrders.Pages.Report
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Order> Orders { get; set; }

        public double Total { get; set; } = 0;

        public async Task<IActionResult> OnGetAsync()
        {
            Orders = await _context.Orders
                .Where(x => EF.Functions.DateDiffDay(x.OrderDate, DateTime.Now) == 0)
                .ToListAsync();



            foreach (var item in Orders)
            {
                var bag = await _context.Bags
                    .Include(x=> x.Meals)
                    .ThenInclude(x=> x.Meal)
                    .FirstOrDefaultAsync(x => x.Id == item.BagId);
                if(bag != null)
                {
                    Total += bag.Meals.Sum(x => x.Quantity * x.Meal.MealPrice);
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostReport()
        {
            var orders = await _context.Orders
                .Where(x => EF.Functions.DateDiffDay(x.OrderDate, DateTime.Now) == 0)
                .ToListAsync();

            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Поръчки");

            worksheet.Cell(1, 1).Value = "Поръчка №";
            worksheet.Cell(1, 2).Value = "Телефон";
            worksheet.Cell(1, 3).Value = "Адрес за доставка";
            worksheet.Cell(1, 4).Value = "Поръчка";
            worksheet.Cell(1, 5).Value = "Обща сума";
            worksheet.Cell(1, 6).Value = "Дата";

            for (int i = 0; i < orders.Count; i++)
            {
                var order = orders[i];
                var bag = await _context.Bags
                    .Include(x => x.Meals)
                    .ThenInclude(x => x.Meal)
                    .FirstOrDefaultAsync(x => x.Id == order.BagId);

                StringBuilder sb = new StringBuilder();

                foreach (var item in bag.Meals)
                {
                    sb.AppendLine($"{item.Meal.MealName} - {item.Quantity} порции");
                }

                worksheet.Cell(i + 2, 1).Value = order.Id.ToString();
                worksheet.Cell(i + 2, 2).Value = order.PhoneNumber;
                worksheet.Cell(i + 2, 3).Value = order.DeliveryAddres;
                worksheet.Cell(i + 2, 4).Value = string.Join(", ", sb.ToString());
                worksheet.Cell(i + 2, 5).Value = bag.Meals.Sum(x => x.Quantity * x.Meal.MealPrice); ;
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
    }
}

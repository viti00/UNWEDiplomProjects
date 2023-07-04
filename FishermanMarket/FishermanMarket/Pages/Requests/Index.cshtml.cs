using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FishermanMarket.Data;
using FishermanMarket.Data.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using System.Security.Claims;
using ClosedXML.Excel;

namespace FishermanMarket.Pages.Requests
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly FishermanMarket.Data.ApplicationDbContext _context;

        public IndexModel(FishermanMarket.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Request> Request { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Requests != null)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (User.IsInRole("Admin") || User.IsInRole("OrdersSender"))
                {
                    Request = await _context.Requests
                    .Include(r => r.User).ToListAsync();
                }
                else
                {
                    Request = await _context.Requests
                    .Include(r => r.User).Where(x => x.UserId == userId).ToListAsync();
                }

            }
        }

        public async Task<IActionResult> OnPostReport()
        {
            var requests = await _context.Requests.Include(x=> x.User).ToListAsync();

            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Заявки");

            worksheet.Cell(1, 1).Value = "Заявка №";
            worksheet.Cell(1, 2).Value = "Имейл";
            worksheet.Cell(1, 4).Value = "Запитване";
            worksheet.Cell(1, 5).Value = "Статус";
            worksheet.Cell(1, 6).Value = "Дата";

            for (int i = 0; i < requests.Count; i++)
            {
                var request = requests[i];
                worksheet.Cell(i + 2, 1).Value = request.Id.ToString();
                worksheet.Cell(i + 2, 2).Value = request.User.Email;
                worksheet.Cell(i + 2, 3).Value = request.Description;
                worksheet.Cell(i + 2, 4).Value = request.Status;
                worksheet.Cell(i + 2, 5).Value = request.RequestDate;
            }

            var fileName = "requests.xlsx";
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

        public async Task<IActionResult> OnPostEditStatus(Guid id)
        {
            var request = await _context.Requests.FirstOrDefaultAsync(x => x.Id == id);

            request.Status = "Отговорено";
            request.LastModified_19118062 = DateTime.Now;

            var log_request = new log_19118062
            {
                Id = Guid.NewGuid(),
                Table = "Requests",
                Operation = "Update",
                Time = DateTime.Now
            };

            _context.log_19118062.Add(log_request);

            await _context.SaveChangesAsync();
            Request = _context.Requests.Include(x => x.User).ToList();
            return Page();
        }

        public async Task OnPostDelete(Guid id)
        {
            var request = await _context.Requests.FirstOrDefaultAsync(x => x.Id == id);

            var log_request = new log_19118062
            {
                Id = Guid.NewGuid(),
                Table = "Requests",
                Operation = "Update",
                Time = DateTime.Now
            };

            _context.log_19118062.Add(log_request);
            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

            Request = _context.Requests.Include(x => x.User).ToList();
            await OnGetAsync();
        }
    }
}

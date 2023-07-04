using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using SteliTour.Data;
using SteliTour.Pages.Destinations;
using SteliTour.Pages.Reservations;

namespace SteliTour.Pages.Requests
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly SteliTour.Data.ApplicationDbContext _context;

        public IndexModel(SteliTour.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Request> Request { get;set; } = default!;

        public int CurrentPage { get; set; } = 1;

        public int EndPage { get; set; }

        public int ElementsPerPage = 9;

        public string Filter { get; set; }
        public SortRequest Sort { get; set; }

        public string Type { get; set; }
        public async Task OnGetAsync(string type, string filter, SortRequest sort, int currPage)
        {
            if(_context.Request != null && type != null && type == "my")
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                Request = await _context.Request
               .Include(r => r.User).Where(x=> x.UserId == userId).ToListAsync();
            }
            else if (_context.Request != null && type != null && type == "admin")
            {
                Request = await _context.Request
                .Include(r => r.User).ToListAsync();
            }

            if(sort == SortRequest.DateAsc)
            {
                Request = Request.OrderBy(x => x.RequestDate).ToList();
            }
            else if(sort == SortRequest.DateDesc)
            {
                Request = Request.OrderByDescending(x => x.RequestDate).ToList();
            }
            if(filter == "W")
            {
                Request = Request.Where(x => x.Status == "W").ToList();
            }
            else if(filter == "A")
            {
                Request = Request.Where(x => x.Status == "A").ToList();
            }

            if(type == "my")
            {
                var lastPage = (int)Math.Ceiling((double)Request.Count / ElementsPerPage);

                if (currPage > lastPage)
                {
                    if (lastPage == 0)
                    {
                        lastPage = 1;
                    }
                    currPage = lastPage;
                }
                if (currPage == 0)
                {
                    currPage = 1;
                }
                EndPage = lastPage;
                CurrentPage = currPage;
                Request = Request.Skip((CurrentPage - 1) * ElementsPerPage)
               .Take(ElementsPerPage).ToList();
            }

            Sort = sort;
            Filter = filter;
            Type = type;

        }

        public async Task<IActionResult> OnPostExport(string filter, SortRequest sort)
        {
            if (filter != null && filter != "All")
            {
                Request = await _context.Request.Where(x => x.Status == filter).ToListAsync();
            }
            else
            {
                Request = await _context.Request.ToListAsync();
            }

            if(sort == SortRequest.DateAsc)
            {
                Request = Request.OrderBy(x => x.RequestDate).ToList();
            }
            else if(sort == SortRequest.DateDesc)
            {
                Request = Request.OrderByDescending(x => x.RequestDate).ToList();
            }

            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Запитвания");

            worksheet.Cell(1, 1).Value = "Запитване №";
            worksheet.Cell(1, 2).Value = "Име";
            worksheet.Cell(1, 3).Value = "Телефон";
            worksheet.Cell(1, 4).Value = "Запитване";
            worksheet.Cell(1, 5).Value = "Статус";
            worksheet.Cell(1, 6).Value = "Дата";

            for (int i = 0; i < Request.Count; i++)
            {
                var req = Request[i];
                worksheet.Cell(i + 2, 1).Value = req.Id.ToString();
                worksheet.Cell(i + 2, 2).Value = req.Name;
                worksheet.Cell(i + 2, 3).Value = req.Phone;
                worksheet.Cell(i + 2, 4).Value = req.RequestText;
                worksheet.Cell(i + 2, 5).Value = req.Status;
                worksheet.Cell(i + 2, 6).Value = req.RequestDate;
            }

            var fileName = "req.xlsx";
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SteliTour.Data;
using SteliTour.Pages.Requests;
using static NuGet.Packaging.PackagingConstants;

namespace SteliTour.Pages.Reservations
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly SteliTour.Data.ApplicationDbContext _context;

        public IndexModel(SteliTour.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Reservation> Reservation { get;set; } = default!;

        public int CurrentPage { get; set; } = 1;

        public int EndPage { get; set; }

        public int ElementsPerPage = 9;

        public string Filter { get; set; }

        public string Type { get; set; }

        public SortReservations Sort { get; set; }


        public async Task OnGetAsync(string type, int currPage, SortReservations sort, string filter)
        {
            ViewData["Destinations"] = new SelectList(_context.Destination.Distinct(), "Id", "DisplayName");

            if (_context.Reservation != null && type != null && type == "my")
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                Reservation = await _context.Reservation
               .Include(r => r.Destination)
               .Include(r => r.User)
               .Where(x => x.UserId == userId).ToListAsync();
            }
            else if (_context.Reservation != null && type != null && type == "admin")
            {
                Reservation = await _context.Reservation
                .Include(r => r.Destination)
                .Include(r => r.User).ToListAsync();
            }

            if (sort == SortReservations.DateAsc)
            {
                Reservation = Reservation.OrderBy(x => x.DateOfReservation).ToList();
            }
            else if (sort == SortReservations.DateDesc)
            {
                Reservation = Reservation.OrderByDescending(x => x.DateOfReservation).ToList();
            }
            if(filter != null && filter != "all")
            {
                var id = Guid.Parse(filter);
                Reservation = Reservation.Where(x => x.DestinationId == id).ToList();
            }

            if(type == "my")
            {
                var lastPage = (int)Math.Ceiling((double)Reservation.Count / ElementsPerPage);

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
                Reservation = Reservation.Skip((CurrentPage - 1) * ElementsPerPage)
               .Take(ElementsPerPage).ToList();
            }

            Sort = sort;
            Filter = filter;
            Type = type;
        }

        public async Task<IActionResult> OnPostExport(string filter, SortReservations sort)
        {
            if(filter != null && filter != "all")
            {
                var id = Guid.Parse(filter);
                Reservation = await _context.Reservation.Include(x=> x.Destination).Where(x => x.DestinationId == id).ToListAsync();
            }
            else
            {
                Reservation = await _context.Reservation.Include(x => x.Destination).ToListAsync();
            }
            if (sort == SortReservations.DateAsc)
            {
                Reservation = Reservation.OrderBy(x => x.DateOfReservation).ToList();
            }
            else if (sort == SortReservations.DateDesc)
            {
                Reservation = Reservation.OrderByDescending(x => x.DateOfReservation).ToList();
            }

            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Резервации");

            worksheet.Cell(1, 1).Value = "Резервация №";
            worksheet.Cell(1, 2).Value = "Дестинация";
            worksheet.Cell(1, 3).Value = "Име";
            worksheet.Cell(1, 4).Value = "ЕГН";
            worksheet.Cell(1, 5).Value = "Телефон";
            worksheet.Cell(1, 6).Value = "Брой възрастни";
            worksheet.Cell(1, 7).Value = "Брой деца";
            worksheet.Cell(1, 8).Value = "Брой деца под 2 години";
            worksheet.Cell(1, 9).Value = "Платени до момента";
            worksheet.Cell(1, 10).Value = "Оставащи да се платят";
            worksheet.Cell(1, 11).Value = "Статус";
            worksheet.Cell(1, 12).Value = "Дата";

            for (int i = 0; i < Reservation.Count; i++)
            {
                var res = Reservation[i];
                worksheet.Cell(i + 2, 1).Value = res.Id.ToString();
                worksheet.Cell(i + 2, 2).Value = res.Destination.Name + " - " + res.Destination.StartDate.ToString("dd.MM.yyyy") + " - " + res.Destination.EndDate.ToString("dd.MM.yyyy");
                worksheet.Cell(i + 2, 3).Value = res.FirstName + " " + res.LastName;
                worksheet.Cell(i + 2, 4).Value = res.EGN;
                worksheet.Cell(i + 2, 5).Value = res.PhoneNumber;
                worksheet.Cell(i + 2, 6).Value = res.AdultCount;
                worksheet.Cell(i + 2, 7).Value = res.KidsCount;
                worksheet.Cell(i + 2, 8).Value = res.Under2YearsKidsCount;
                worksheet.Cell(i + 2, 9).Value = res.Payed;
                worksheet.Cell(i + 2, 10).Value = res.Remainign;
                worksheet.Cell(i + 2, 11).Value = res.Status;
                worksheet.Cell(i + 2, 12).Value = res.DateOfReservation;
            }

            var fileName = "res.xlsx";
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

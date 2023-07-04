using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SteliTour.Data;
using SteliTour.Pages.Reservations;

namespace SteliTour.Pages.DenidedReservations
{
    [Authorize(Roles = "Admin, Worker")]
    public class IndexModel : PageModel
    {
        private readonly SteliTour.Data.ApplicationDbContext _context;

        public IndexModel(SteliTour.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<DenidedReservation> DenidedReservation { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.DenidedReservations != null)
            {
                DenidedReservation = await _context.DenidedReservations.ToListAsync();
            }
        }

        public async Task<IActionResult> OnPostExport()
        {
            DenidedReservation = _context.DenidedReservations.ToList();

            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Отказани резервации");

            worksheet.Cell(1, 1).Value = "Резервация №";
            worksheet.Cell(1, 2).Value = "Дестинация";
            worksheet.Cell(1, 3).Value = "Име";
            worksheet.Cell(1, 4).Value = "ЕГН";
            worksheet.Cell(1, 5).Value = "Телефон";
            worksheet.Cell(1, 6).Value = "Имейл";
            worksheet.Cell(1, 7).Value = "Брой възрастни";
            worksheet.Cell(1, 8).Value = "Брой деца";
            worksheet.Cell(1, 9).Value = "Брой деца под 2 години";
            worksheet.Cell(1, 10).Value = "Платени до момента";
            worksheet.Cell(1, 11).Value = "Оставащи да се платят";
            worksheet.Cell(1, 12).Value = "Дата на резервация";
            worksheet.Cell(1, 13).Value = "Дата на анулиране";

            for (int i = 0; i < DenidedReservation.Count; i++)
            {
                var res = DenidedReservation[i];
                worksheet.Cell(i + 2, 1).Value = res.Id.ToString();
                worksheet.Cell(i + 2, 2).Value = res.Destination;
                worksheet.Cell(i + 2, 3).Value = res.FirstName + " " + res.LastName;
                worksheet.Cell(i + 2, 4).Value = res.EGN;
                worksheet.Cell(i + 2, 5).Value = res.PhoneNumber;
                worksheet.Cell(i + 2, 6).Value = res.UserEmail;
                worksheet.Cell(i + 2, 7).Value = res.AdultCount;
                worksheet.Cell(i + 2, 8).Value = res.KidsCount;
                worksheet.Cell(i + 2, 9).Value = res.Under2YearsKidsCount;
                worksheet.Cell(i + 2, 10).Value = res.Payed;
                worksheet.Cell(i + 2, 11).Value = res.Remainign;
                worksheet.Cell(i + 2, 12).Value = res.DateOfReservation;
                worksheet.Cell(i + 2, 13).Value = res.DateOfCancelation;
            }

            var fileName = "denided.xlsx";
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

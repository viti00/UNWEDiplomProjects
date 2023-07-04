namespace FitnessProgram.Services.CustomerService
{
    using ClosedXML.Excel;
    using FitnessProgram.Data;
    using FitnessProgram.Data.Models;
    using FitnessProgram.ViewModels.Customer;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Globalization;
    using static FitnessProgram.Services.SharedMethods;

    public class CustomerService : ICustomerService
    {
        private readonly FitnessProgramDbContext context;

        public CustomerService(FitnessProgramDbContext context)
            => this.context = context;

        public List<ReservationDateTimeRange> GetReservation(string id)
        {
            try
            {
                var dates = context.ReservationDateTimeRange.Where(x => x.DateOfReservationId == id).ToList();

                if (dates.Count == 0)
                {
                    var d = new DateOfReservation
                    {
                        Id = id
                    };
                    context.DateOfReservations.Add(d);
                    foreach (var range in context.TimeRanges.ToList())
                    {
                        var date = new ReservationDateTimeRange
                        {
                            DateOfReservationId = id,
                            TimeRangeId = range.Id,
                            Status = "Free",
                            LastModified_19118074 = DateTime.Now
                        };
                        dates.Add(date);
                    }

                    context.log_19118074.Add(CreateLog("ReservationDateTimeRanges", "Insert"));
                    context.ReservationDateTimeRange.AddRange(dates);
                    context.SaveChanges();
                }


                return dates;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
           
        }

        public bool CreateReservation(ReservationFormModel model, string userId)
        {
            try
            {
                var timeRange = GetCurrentTimeRange(model.DateId, model.RangeId);

                if (timeRange != null && timeRange.Status == "Free")
                {
                    DateTime date = DateTime.Parse(model.DateId);
                    DateTime time = DateTime.ParseExact(timeRange.TimeRange.RangeName.Split(" - ")[0], "HH:mm", CultureInfo.InvariantCulture);

                    DateTime result = new DateTime(date.Year, date.Month, date.Day, time.Hour - 1, time.Minute, time.Second);
                    var reservation = new Reservation
                    {
                        DateId = model.DateId,
                        RangeId = model.RangeId,
                        FullName = model.Name,
                        PhoneNumber = model.Phone,
                        Address = model.Address,
                        UserId = userId,
                        User = context.Users.FirstOrDefault(x => x.Id == userId),
                        LastModified_19118074 = DateTime.Now,
                        LastAvailableForModification = result
                    };

                    context.log_19118074.Add(CreateLog("Reservations", "Insert"));
                    context.Reservations.Add(reservation);
                    timeRange.Status = "Taken";
                    context.SaveChanges();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
        }

        public ReservationDateTimeRange GetCurrentTimeRange(string dateId, int rangeId)
        {
            try
            {
                var timeRange = context.ReservationDateTimeRange
                    .Include(x => x.TimeRange)
                    .Include(x => x.DateOfReservation)
                    .FirstOrDefault(x => x.DateOfReservationId == dateId && x.TimeRangeId == context.TimeRanges.FirstOrDefault(x => x.RangeId == rangeId).Id);

                return timeRange;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
        }

        public List<ReservationViewModel> GetMyReservations(string userId)
        {
            try
            {
                var reservations = context.Reservations.Where(x => x.UserId == userId)
                .Select(x => new ReservationViewModel
                {
                    Id = x.Id,
                    Date = x.DateId,
                    Name = x.FullName,
                    Address = x.Address,
                    Phone = x.PhoneNumber,
                    TimeRange = context.TimeRanges.FirstOrDefault(r => r.RangeId == x.RangeId).RangeName,
                    LastAvailableForModification = x.LastAvailableForModification
                }).ToList();

                return reservations;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
        }

        public bool EditReservation(string resId, string name, string address, string phone)
        {
            try
            {
                var reservation = context.Reservations.FirstOrDefault(x => x.Id == resId);

                if (reservation != null)
                {
                    reservation.Address = address;
                    reservation.PhoneNumber = phone;
                    reservation.FullName = name;
                    reservation.LastModified_19118074 = DateTime.Now;

                    context.log_19118074.Add(CreateLog("Reservation", "Update"));
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
        }

        public bool RejectReservation(string resId)
        {
            try
            {
                var reservation = context.Reservations.FirstOrDefault(x => x.Id == resId);

                if (reservation != null)
                {
                    var timeRange = context.ReservationDateTimeRange
                        .FirstOrDefault(x => x.DateOfReservationId == reservation.DateId && x.TimeRangeId == context.TimeRanges.FirstOrDefault(x => x.RangeId == reservation.RangeId).Id);

                    if (timeRange != null)
                    {
                        timeRange.Status = "Free";
                        timeRange.LastModified_19118074 = DateTime.Now;
                    }

                    context.log_19118074.Add(CreateLog("TimeRanges", "Update"));

                    context.Reservations.Remove(reservation);

                    context.log_19118074.Add(CreateLog("Reservations", "Update"));

                    context.SaveChanges();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
        }

        public List<AllReservationViewModel> GetAllReservations(string dateOfReservationId)
        {
            try
            {
                var reservations = context.Reservations.Include(x=> x.User)
                .Where(x => x.DateId == dateOfReservationId)
                .Select(x => new AllReservationViewModel
                {
                    ReservationId = x.Id,
                    RangeId = context.TimeRanges.FirstOrDefault(r => r.RangeId == x.RangeId).RangeId,
                    Name = x.FullName,
                    Address = x.Address,
                    Phone = x.PhoneNumber,
                    Email = x.User.Email
                }).ToList();

                return reservations;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
        }
        private List<Reservation> GetAll()
        {
            return context.Reservations.ToList();
        }
        public ExportModel ExportToExcel()
        {
            try
            {
                var toExport = GetAll();

                var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Reservations");

                worksheet.Cell(1, 1).Value = "Name";
                worksheet.Cell(1, 2).Value = "Address";
                worksheet.Cell(1, 3).Value = "Phone";
                worksheet.Cell(1, 4).Value = "Date";
                worksheet.Cell(1, 5).Value = "TimeRange";

                for (int i = 0; i < toExport.Count; i++)
                {
                    var res = toExport[i];
                    worksheet.Cell(i + 2, 1).Value = res.FullName;
                    worksheet.Cell(i + 2, 2).Value = res.Address;
                    worksheet.Cell(i + 2, 3).Value = res.PhoneNumber;
                    worksheet.Cell(i + 2, 4).Value = res.DateId;
                    worksheet.Cell(i + 2, 5).Value = context.TimeRanges.FirstOrDefault(x => x.Id == res.RangeId).RangeName;
                }

                var fileName = "reservations.xlsx";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileName);
                workbook.SaveAs(filePath);

                var memory = new MemoryStream();
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    stream.CopyTo(memory);
                }
                memory.Position = 0;

                var expModel = new ExportModel
                {
                    Memory = memory,
                    FileName = fileName,
                };

                return expModel;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
        }

        public Reservation GetReservationById(string resId)
        {
            var res = context.Reservations.FirstOrDefault(x => x.Id == resId);

            return res;
        }
    }
}

using FitnessProgram.Data;
using FitnessProgram.Data.Models;
using static FitnessProgram.Services.SharedMethods;

namespace FitnessProgram.Infrastructure
{
    public class DbInitializer
    {
        public static void Initialize(FitnessProgramDbContext context)
        {
            SeedTimeRanges(context);
        }

        private static void SeedTimeRanges(FitnessProgramDbContext context)
        {
            try
            {
                if (!context.TimeRanges.Any())
                {
                    var timeRanges = new List<TimeRange>
                {
                    new TimeRange{RangeId = 0, RangeName="7:00 - 8:00" },
                    new TimeRange{RangeId = 1, RangeName="9:00 - 10:00"},
                    new TimeRange{RangeId = 2, RangeName="11:00 - 12:00"},
                    new TimeRange{RangeId = 3, RangeName="13:00 - 14:00"},
                    new TimeRange{RangeId = 4, RangeName="15:00 - 16:00"},
                    new TimeRange{RangeId = 5, RangeName="17:00 - 18:00"},
                    new TimeRange{RangeId = 6, RangeName="19:00 - 20:00"},
                };

                    context.TimeRanges.AddRange(timeRanges);

                    foreach (var item in timeRanges)
                    {
                        context.log_19118074.Add(CreateLog("TimeRanges", "Insert"));
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
        }
        
    }
}

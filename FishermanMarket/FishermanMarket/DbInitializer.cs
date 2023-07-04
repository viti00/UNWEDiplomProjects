using FishermanMarket.Data;
using FishermanMarket.Data.Models;

namespace FishermanMarket
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            SeedCategory(context);
        }

        private static void SeedCategory(ApplicationDbContext context)
        {
            if (!context.Categories.Any())
            {
                var categories = new List<Category>()
                {
                    new Category {Name="Въдици",  LastModified_19118062 = DateTime.Now},
                    new Category {Name="Макари",  LastModified_19118062 = DateTime.Now},
                    new Category {Name="Влакна",  LastModified_19118062 = DateTime.Now},
                    new Category {Name="Примамки",  LastModified_19118062 = DateTime.Now},
                };

                var log = new log_19118062
                {
                    Table = "Categories",
                    Operation = "Insert",
                    Time = DateTime.Now
                };
                context.log_19118062.Add(log);

                context.Categories.AddRange(categories);
                context.SaveChanges();
            }
        }
    }
}

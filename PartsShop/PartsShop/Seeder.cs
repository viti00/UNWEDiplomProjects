using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PartsShop.Data;
using System.Diagnostics.Eventing.Reader;

namespace PartsShop
{
    public class Seeder
    {
        public void SeedCategories(IServiceProvider services)
        {
            var _context = services.GetRequiredService<ApplicationDbContext>();

            if (!_context.Categories.Any())
            {
                var categories = new List<Category>()
                {
                    new Category {LastModified_19118073=DateTime.Now, CategoryName = "Спирачна система"},
                    new Category {LastModified_19118073=DateTime.Now, CategoryName = "Окачване"},
                    new Category {LastModified_19118073=DateTime.Now, CategoryName = "Кормилна система"},
                    new Category {LastModified_19118073=DateTime.Now, CategoryName = "Части за двигател"},
                    new Category {LastModified_19118073=DateTime.Now, CategoryName = "Датчици"},
                    new Category {LastModified_19118073=DateTime.Now, CategoryName = "Горивна система"},
                    new Category {LastModified_19118073=DateTime.Now, CategoryName = "Ауспуси и гърнета"},
                    new Category {LastModified_19118073=DateTime.Now, CategoryName = "Стартова система"},
                    new Category {LastModified_19118073=DateTime.Now, CategoryName = "Светлини"}
                };



                foreach (var category in categories)
                {
                    _context.Categories.Add(category);
                    var log = new log_19118073
                    {
                        Id = Guid.NewGuid(),
                        Table = "PartType",
                        Action = "Insert",
                        ActionTime = DateTime.Now
                    };
                    _context.log_19118073.Add(log);
                }
                _context.SaveChanges();
            }
        }

        public async Task SeedAdmin(IServiceProvider services)
        {
            var _roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var _userManager = services.GetRequiredService<UserManager<IdentityUser>>();

                if (await _roleManager.RoleExistsAsync("Admin"))
                {
                    return;
                }

                var role = new IdentityRole { Name = "Admin" };

                await _roleManager.CreateAsync(role);

                const string email = "admin19118973@abv.bg";
                const string username = "admin19118073@abv.bg";
                const string password = "admin19118073";

                var user = new IdentityUser
                {
                    Email = email,
                    UserName = username,
                };

                await _userManager.CreateAsync(user, password);

                await _userManager.AddToRoleAsync(user, role.Name);
        }

        public async Task SeedOrdersManager(IServiceProvider services)
        {
            var _roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var _userManager = services.GetRequiredService<UserManager<IdentityUser>>();

            if (await _roleManager.RoleExistsAsync("OrdersManager"))
            {
                return;
            }

            var role = new IdentityRole { Name = "OrdersManager" };

            await _roleManager.CreateAsync(role);

            const string email = "orders19118073@abv.bg";
            const string username = "orders19118073@abv.bg";
            const string password = "orders19118073";

            var user = new IdentityUser
            {
                Email = email,
                UserName = username,
            };

            await _userManager.CreateAsync(user, password);

            await _userManager.AddToRoleAsync(user, role.Name);
        }

        public async Task SeedInitial(IServiceProvider services)
        {
            var _context = services.GetRequiredService<ApplicationDbContext>();
            var _userManager = services.GetRequiredService<UserManager<IdentityUser>>();


            if (!_context.Users.Any(x => x.UserName == "initialuser@abv.bg"))
            {

                const string email = "initialuser@abv.bg";
                const string username = "initialuser@abv.bg";
                const string password = "initialuser";

                var user = new IdentityUser
                {
                    Email = email,
                    UserName = username,
                };

                await _userManager.CreateAsync(user, password);
            }

        }
    }
}

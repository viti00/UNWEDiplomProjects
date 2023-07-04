using Microsoft.AspNetCore.Identity;
using RestaurantOrders.Data;

namespace RestaurantOrders
{
    public class GLobalMethods
    {
        public void AddCategories(IServiceProvider services)
        {
            var _context = services.GetService<ApplicationDbContext>();

            if (!_context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category{Name ="Салати", LastModified_19118066 = DateTime.Now},
                    new Category{Name ="Предястия", LastModified_19118066 = DateTime.Now},
                    new Category{Name ="Основни", LastModified_19118066 = DateTime.Now},
                    new Category{Name ="Десерти", LastModified_19118066 = DateTime.Now},
                    new Category{Name ="Напитки", LastModified_19118066 = DateTime.Now},
                    new Category{Name ="Алкохол", LastModified_19118066 = DateTime.Now},
                };

                foreach (var item in categories)
                {
                    var log = new log_19118066
                    {
                        Id = Guid.NewGuid(),
                        TableName = "Categories",
                        CommandType = "Insert",
                        ExecutionDate = DateTime.Now
                    };
                    _context.log_19118066.Add(log);
                    _context.Categories.Add(item);
                }

                _context.SaveChanges();
            }
        }

        public void AddAdministrator(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task
                .Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync("Administrator"))
                    {
                        return;
                    }

                    var role = new IdentityRole { Name = "Administrator" };

                    await roleManager.CreateAsync(role);

                    const string email = "admin@admin.com";
                    const string username = "admin@admin.com";
                    const string password = "admin123";

                    var user = new IdentityUser
                    {
                        Email = email,
                        UserName = username,
                    };

                    await userManager.CreateAsync(user, password);

                    await userManager.AddToRoleAsync(user, role.Name);
                })
                .GetAwaiter()
                .GetResult();
        }

        public void AddCooker(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task
                .Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync("Cooker"))
                    {
                        return;
                    }

                    var role = new IdentityRole { Name = "Cooker" };

                    await roleManager.CreateAsync(role);

                    const string email = "cooker@cooker.com";
                    const string username = "cooker@cooker.com";
                    const string password = "cooker123";

                    var user = new IdentityUser
                    {
                        Email = email,
                        UserName = username,
                    };

                    await userManager.CreateAsync(user, password);

                    await userManager.AddToRoleAsync(user, role.Name);
                })
                .GetAwaiter()
                .GetResult();
        }

        public void AddDeliveryGuy(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task
                .Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync("DeliveryGuy"))
                    {
                        return;
                    }

                    var role = new IdentityRole { Name = "DeliveryGuy" };

                    await roleManager.CreateAsync(role);

                    const string email = "delivery@delivery.com";
                    const string username = "delivery@delivery.com";
                    const string password = "delivery123";

                    var user = new IdentityUser
                    {
                        Email = email,
                        UserName = username,
                    };

                    await userManager.CreateAsync(user, password);

                    await userManager.AddToRoleAsync(user, role.Name);
                })
                .GetAwaiter()
                .GetResult();
        }

        public void AddUser(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            var _context = services.GetService<ApplicationDbContext>();
            Task
                .Run(async () =>
                {
                    if (_context.Users.FirstOrDefault(x => x.Email == "user@user.com") == null)
                    {
                        const string email = "user@user.com";
                        const string username = "user@user.com";
                        const string password = "user123";

                        var user = new IdentityUser
                        {
                            Email = email,
                            UserName = username,
                        };
                        await userManager.CreateAsync(user, password);
                    }
                })
                .GetAwaiter()
                .GetResult();
        }
    }
}

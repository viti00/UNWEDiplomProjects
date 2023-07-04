using Microsoft.AspNetCore.Identity;
using VBoutique.Data;

namespace VBoutique
{
    public class DbInitializer
    {
        public void AddColors(IServiceProvider services)
        {
            var _contex = services.GetRequiredService<ApplicationDbContext>();

            if (!_contex.Colors.Any())
            {
                var colors = new List<Color>()
                {
                    new Color{ColorNameBg= "Син", ColorNameEn="Blue", LastModified_19118155 = DateTime.Now},
                    new Color{ColorNameBg= "Черен", ColorNameEn="Black", LastModified_19118155 = DateTime.Now},
                    new Color{ColorNameBg= "Червен", ColorNameEn="Red", LastModified_19118155 = DateTime.Now},
                    new Color{ColorNameBg= "Жълт", ColorNameEn="Yellow", LastModified_19118155 = DateTime.Now},
                    new Color{ColorNameBg= "Бял", ColorNameEn="White", LastModified_19118155 = DateTime.Now},
                    new Color{ColorNameBg= "Оранжев", ColorNameEn="Orange", LastModified_19118155 = DateTime.Now},
                    new Color{ColorNameBg= "Сив", ColorNameEn="Grey", LastModified_19118155 = DateTime.Now},
                    new Color{ColorNameBg= "Зелен", ColorNameEn="Green", LastModified_19118155 = DateTime.Now},
                };

                _contex.Colors.AddRange(colors);
                foreach (var item in colors)
                {
                    var log = new log_19118155
                    {
                        Id = Guid.NewGuid(),
                        TableName = "Colors",
                        OperationType = "Insert",
                        OperationTime = DateTime.Now
                    };
                    _contex.log_19118155.Add(log);
                }

                _contex.SaveChanges();
            }
        }

        public void AddSizes(IServiceProvider services)
        {
            var _contex = services.GetRequiredService<ApplicationDbContext>();
            if (!_contex.Sizes.Any())
            {
                var sizes = new List<Size>()
                {
                    new Size{Value = "35", LastModified_19118155=DateTime.Now},
                    new Size{Value = "36", LastModified_19118155=DateTime.Now},
                    new Size{Value = "37", LastModified_19118155=DateTime.Now},
                    new Size{Value = "38", LastModified_19118155=DateTime.Now},
                    new Size{Value = "39", LastModified_19118155=DateTime.Now},
                    new Size{Value = "40", LastModified_19118155=DateTime.Now},
                    new Size{Value = "41", LastModified_19118155=DateTime.Now},
                };

                _contex.Sizes.AddRange(sizes);
                foreach (var item in sizes)
                {
                    var log = new log_19118155
                    {
                        Id = Guid.NewGuid(),
                        TableName = "Sizes",
                        OperationType = "Insert",
                        OperationTime = DateTime.Now
                    };
                    _contex.log_19118155.Add(log);
                }

                _contex.SaveChanges();
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

            const string email = "vkitanova@admin.bg";
            const string username = "vkitanova@admin.bg";
            const string password = "vkitanova19118155";

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

            const string email = "vkitanova@orders.bg";
            const string username = "vkitanova@orders.bg";
            const string password = "vkitanova19118155";

            var user = new IdentityUser
            {
                Email = email,
                UserName = username,
            };

            await _userManager.CreateAsync(user, password);

            await _userManager.AddToRoleAsync(user, role.Name);
        }

        public async Task SeedProductsManager(IServiceProvider services)
        {
            var _roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var _userManager = services.GetRequiredService<UserManager<IdentityUser>>();

            if (await _roleManager.RoleExistsAsync("ProductsManager"))
            {
                return;
            }

            var role = new IdentityRole { Name = "ProductsManager" };

            await _roleManager.CreateAsync(role);

            const string email = "vkitanova@products.bg";
            const string username = "vkitanova@products.bg";
            const string password = "vkitanova19118155";

            var user = new IdentityUser
            {
                Email = email,
                UserName = username,
            };

            await _userManager.CreateAsync(user, password);

            await _userManager.AddToRoleAsync(user, role.Name);
        }

        public async Task SeedCustomer(IServiceProvider services)
        {
            var _context = services.GetRequiredService<ApplicationDbContext>();
            var _userManager = services.GetRequiredService<UserManager<IdentityUser>>();

            if(!_context.Users.Any(x=> x.UserName == "vkitanova@customer.bg"))
            {
                const string email = "vkitanova@customer.bg";
                const string username = "vkitanova@customer.bg";
                const string password = "vkitanova19118155";

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

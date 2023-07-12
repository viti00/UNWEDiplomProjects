using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FoodAdditivesShop.Data;
using System.Diagnostics.Eventing.Reader;

namespace FoodAdditivesShop
{
    public class Seeder
    {
        
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

                const string email = "admin19118119@abv.bg";
                const string username = "admin19118119@abv.bg";
                const string password = "admin19118119";

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

            const string email = "orders19118119@abv.bg";
            const string username = "orders19118119@abv.bg";
            const string password = "orders19118119";

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


            if (!_context.Users.Any(x => x.UserName == "user19118119@abv.bg"))
            {

                const string email = "user19118119@abv.bg";
                const string username = "user19118119@abv.bg";
                const string password = "user19118119";

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

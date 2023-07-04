using Microsoft.AspNetCore.Identity;
using SteliTour.Data;
using Stripe;

namespace SteliTour
{
    public class GlobalMehods
    {
        public void AddAdministrator(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task
                .Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync("Admin"))
                    {
                        return;
                    }

                    var role = new IdentityRole { Name = "Admin" };

                    await roleManager.CreateAsync(role);

                    const string email = "admin@abv.com";
                    const string username = "admin";
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

        public void AddWorker(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task
                .Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync("Worker"))
                    {
                        return;
                    }

                    var role = new IdentityRole { Name = "Worker" };

                    await roleManager.CreateAsync(role);

                    const string email = "worker@abv.com";
                    const string username = "worker";
                    const string password = "worker123";

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
                    if (_context.Users.FirstOrDefault(x => x.Email == "user@abv.com") == null)
                    {
                        const string email = "user@abv.com";
                        const string username = "user";
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

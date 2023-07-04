using CarPartsShop.Data;
using CarPartsShop.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static CarPartsShop.Infrastructure.RolesConstants;

namespace CarPartsShop.Infrastructure
{
    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder PrepareDatabase(
                this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;
            AddAdminRole(services);
            AddWorkerRole(services);
            AddDefaultUser(services);


            MigrateDatabase(services);

            return app;
        }

        private static void MigrateDatabase(IServiceProvider services)
        {
            var data = services.GetRequiredService<CarPartsShopDbContext>();
            DbInitializer.Initialize(data);
            //data.Database.Migrate();
        }

        private static void AddAdminRole(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task
                .Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync(Administrator))
                    {
                        return;
                    }

                    var role = new IdentityRole { Name = Administrator };

                    await roleManager.CreateAsync(role);

                    const string username = "admin";
                    const string password = "admin123456";
                    const string email = "admin@admin.bg";

                    var user = new ApplicationUser
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

        private static void AddWorkerRole(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task
                .Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync(Worker))
                    {
                        return;
                    }

                    var role = new IdentityRole { Name = Worker };

                    await roleManager.CreateAsync(role);

                    const string username = "worker";
                    const string password = "worker123456";
                    const string email = "worker@worker.bg";

                    

                    var user = new ApplicationUser
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
        private static void AddDefaultUser(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task
                .Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync(Default))
                    {
                        return;
                    }

                    var role = new IdentityRole { Name = Default };

                    await roleManager.CreateAsync(role);

                    const string username = "user";
                    const string password = "user123456";
                    const string email = "user@user.bg";



                    var user = new ApplicationUser
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
    }
}

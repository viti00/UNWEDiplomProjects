namespace FitnessProgram.Infrastructure
{
    using FitnessProgram.Data;
    using FitnessProgram.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using static FitnessProgram.WebConstants;

    public static class ApplicationBuilderExtension
    {
        public static async Task<IApplicationBuilder> PrepareDatabase(
                this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;

            MigrateDatabase(services);

            await SeedAdministrator(services);
            await SeedModerator(services);
            await SeedDefaultUser(services);

            return app;
        }

        private static void MigrateDatabase(IServiceProvider services)
        {
            var data = services.GetRequiredService<FitnessProgramDbContext>();
            DbInitializer.Initialize(data);
        }

        private static async Task SeedAdministrator(IServiceProvider services)
        {
            try
            {
                var userManager = services.GetRequiredService<UserManager<User>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();


                if (await roleManager.RoleExistsAsync(AdministratorRoleName))
                {
                    return;
                }

                var role = new IdentityRole { Name = AdministratorRoleName };

                await roleManager.CreateAsync(role);

                const string adminEmail = "admin@fp.com";
                const string adminUsername = "admin";
                const string adminPassword = "123admin123";

                var user = new User
                {
                    Email = adminEmail,
                    UserName = adminUsername,
                };

                await userManager.CreateAsync(user, adminPassword);

                await userManager.AddToRoleAsync(user, role.Name);

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
        private static async Task SeedModerator(IServiceProvider services)
        {
            try
            {
                var userManager = services.GetRequiredService<UserManager<User>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();


                if (await roleManager.RoleExistsAsync(ModeratorRoleName))
                {
                    return;
                }

                var role = new IdentityRole { Name = ModeratorRoleName };

                await roleManager.CreateAsync(role);

                const string moderatorEmail = "moderator@fp.com";
                const string moderatorUsername = "moderator";
                const string moderatorPassword = "123moderator123";

                var user = new User
                {
                    Email = moderatorEmail,
                    UserName = moderatorUsername,
                };

                await userManager.CreateAsync(user, moderatorPassword);

                await userManager.AddToRoleAsync(user, role.Name);

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
        private static async Task SeedDefaultUser(IServiceProvider services)
        {
            try
            {
                var userManager = services.GetRequiredService<UserManager<User>>();
                var context = services.GetRequiredService<FitnessProgramDbContext>();

                if (!context.Users.Any(x => x.Email == "user@fp.com" && x.UserName == "defaultuser"))
                {

                    const string userEmail = "user@fp.com";
                    const string userUsername = "defaultuser";
                    const string userPassword = "123user123";

                    var user = new User
                    {
                        Email = userEmail,
                        UserName = userUsername,
                    };

                    await userManager.CreateAsync(user, userPassword);

                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
    }
}

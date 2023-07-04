using FishermanMarket;
using FishermanMarket.Data;
using FishermanMarket.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.SignIn.RequireConfirmedAccount = false;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();
builder.Services.AddMvc().AddRazorPagesOptions(o =>
{
    o.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
});

var app = builder.Build();

using (var serviceScope = app.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;
    var context = services.GetService<ApplicationDbContext>();
    DbInitializer.Initialize(context);

    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    
    Task.Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync("OrdersSender"))
                    {
                        return;
                    }

                    var role = new IdentityRole { Name = "OrdersSender" };

                    await roleManager.CreateAsync(role);

                    const string username = "sender@abv.bg";
                    const string password = "sender";
                    const string email = "sender@abv.bg";



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

    Task.Run(async () =>
    {
        if (await roleManager.RoleExistsAsync("Admin"))
        {
            return;
        }

        var role = new IdentityRole { Name = "Admin" };

        await roleManager.CreateAsync(role);

        const string username = "admin@abv.bg";
        const string password = "admin123";
        const string email = "admin@abv.bg";

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

    Task.Run(async () =>
    {
        if(context.Users.FirstOrDefault(x=> x.Email=="testuser@abv.bg") == null)
        {
            const string username = "testuser@abv.bg";
            const string password = "testuser";
            const string email = "testuser@abv.bg";

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();

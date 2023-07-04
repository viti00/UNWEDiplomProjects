using HomeEssentials.Data;
using Microsoft.AspNetCore.Identity;

namespace HomeEssentials
{
    public class DbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public DbInitializer(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public void AddCategories()
        {
            if (!_context.ItemCategories.Any())
            {
                var categories = new List<ItemCategory>
                {
                    new ItemCategory {Category = "ТВ, Аудио и Електроника", LastModified_19118075=DateTime.Now},
                    new ItemCategory {Category = "Телефони и Таблети", LastModified_19118075=DateTime.Now},
                    new ItemCategory {Category = "Лаптопи, компютри и периферия", LastModified_19118075=DateTime.Now},
                    new ItemCategory {Category = "Домакински Електроуреди", LastModified_19118075=DateTime.Now},
                    new ItemCategory {Category = "Уреди за здраве и красота", LastModified_19118075=DateTime.Now},
                    new ItemCategory {Category = "Климатици, уреди за отопление и грижа за въздуха", LastModified_19118075=DateTime.Now},
                    new ItemCategory {Category = "Дом и Градина", LastModified_19118075=DateTime.Now},
                    new ItemCategory {Category = "Спорт и Свободно време", LastModified_19118075=DateTime.Now},
                    new ItemCategory {Category = "Фото и Видео", LastModified_19118075=DateTime.Now},
                    new ItemCategory {Category = "Играчки и Детски артикули", LastModified_19118075=DateTime.Now},
                };

                foreach (var item in categories)
                {
                    var log = new log_19118075
                    {
                        Id = Guid.NewGuid(),
                        Table = "Categories",
                        Command = "Insert",
                        DateTime = DateTime.Now
                    };

                    _context.log_19118075.Add(log);
                    _context.ItemCategories.Add(item);
                    _context.SaveChanges();
                }
            }
        }
        public void AddCartStatuses()
        {
            if (!_context.CartStatuses.Any())
            {
                var statuses = new List<CartStatus>
                {
                    new CartStatus {Name="Active", LastModified_19118075=DateTime.Now},
                    new CartStatus {Name="Ordered", LastModified_19118075=DateTime.Now}
                };

                foreach (var item in statuses)
                {
                    var log = new log_19118075
                    {
                        Id = Guid.NewGuid(),
                        Table = "CartStatuses",
                        Command = "Insert",
                        DateTime = DateTime.Now
                    };

                    _context.log_19118075.Add(log);
                    _context.CartStatuses.Add(item);
                    _context.SaveChanges();
                }
            }
        }

        public void AddOrderStatuses()
        {
            if (!_context.OrderStatuses.Any())
            {
                var statuses = new List<OrderStatuses>
                {
                    new OrderStatuses {Name="Awaiting", LastModified_19118075=DateTime.Now},
                    new OrderStatuses {Name="Sent", LastModified_19118075=DateTime.Now},
                    new OrderStatuses {Name="Delivered", LastModified_19118075=DateTime.Now},
                    new OrderStatuses {Name="Returned", LastModified_19118075=DateTime.Now},
                    new OrderStatuses {Name="Rejected", LastModified_19118075=DateTime.Now},
                };

                foreach (var item in statuses)
                {
                    var log = new log_19118075
                    {
                        Id = Guid.NewGuid(),
                        Table = "OrderStatuses",
                        Command = "Insert",
                        DateTime = DateTime.Now
                    };

                    _context.log_19118075.Add(log);
                    _context.OrderStatuses.Add(item);
                    _context.SaveChanges();
                }
            }
        }
        public void AddAdministrator()
        {
            Task
               .Run(async () =>
               {
                   if (await _roleManager.RoleExistsAsync("Administrator"))
                   {
                       return;
                   }

                   var role = new IdentityRole { Name = "Administrator" };

                   await _roleManager.CreateAsync(role);

                   const string email = "jivkoadmin@abv.bg";
                   const string username = "jivkoadmin@abv.bg";
                   const string password = "jivko19118075";

                   var user = new IdentityUser
                   {
                       Email = email,
                       UserName = username,
                   };

                   await _userManager.CreateAsync(user, password);

                   await _userManager.AddToRoleAsync(user, role.Name);
               })
               .GetAwaiter()
               .GetResult();
        }

        public void AddOrdersManager()
        {
            Task
               .Run(async () =>
               {
                   if (await _roleManager.RoleExistsAsync("OrdersManager"))
                   {
                       return;
                   }

                   var role = new IdentityRole { Name = "OrdersManager" };

                   await _roleManager.CreateAsync(role);

                   const string email = "jivkoorders@abv.bg";
                   const string username = "jivkoorders@abv.bg";
                   const string password = "jivko19118075";

                   var user = new IdentityUser
                   {
                       Email = email,
                       UserName = username,
                   };

                   await _userManager.CreateAsync(user, password);

                   await _userManager.AddToRoleAsync(user, role.Name);
               })
               .GetAwaiter()
               .GetResult();
        }

        public void AddDefaultUser()
        {
            if(!_context.Users.Any(x=> x.UserName == "jivkouser@abv.bg"))
            {
                Task
               .Run(async () =>
               {
                   const string email = "jivkouser@abv.bg";
                   const string username = "jivkouser@abv.bg";
                   const string password = "jivko19118075";

                   var user = new IdentityUser
                   {
                       Email = email,
                       UserName = username,
                   };

                   await _userManager.CreateAsync(user, password);
               })
               .GetAwaiter()
               .GetResult();
            }
            
        }
    }
}

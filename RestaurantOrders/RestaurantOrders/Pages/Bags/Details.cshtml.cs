using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RestaurantOrders.Data;

namespace RestaurantOrders.Pages.Bags
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly RestaurantOrders.Data.ApplicationDbContext _context;

        public DetailsModel(RestaurantOrders.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Bag Bag { get; set; } = default!;

        public string Type { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid bId, string type)
        {
            Bag? bag = null;
            if(type != null && type == "ordered")
            {
                bag = await _context.Bags
                 .Include(x => x.User)
                .Include(x => x.Meals)
                .ThenInclude(x => x.Meal)
                .FirstOrDefaultAsync(x => x.Id == bId);
            }
            else
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                bag = await _context
                    .Bags
                    .Include(x => x.User)
                    .Include(x => x.Meals)
                    .ThenInclude(x => x.Meal)
                    .FirstOrDefaultAsync(x => x.UserId == userId && x.Status == "A");
            }



            Type = type;
            Bag = bag;

            return Page();
        }

        public async Task<IActionResult> OnGetAddToBag(Guid pId, int quantity)
        {
            var message = "";
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (quantity < 1)
            {
                message = "Трябва да добавите поне 1 бройка от дадения продукт!";
                return new JsonResult(message);
            }
            
            var bag = await _context
                .Bags
                .Include(x => x.User)
                .Include(x => x.Meals)
                .ThenInclude(x => x.Meal)
                .FirstOrDefaultAsync(x => x.UserId == userId && x.Status == "A");

            if (bag == null)
            {
                bag = await CreateBag(userId);
            }

            var product = await _context.Meals.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == pId);

            if (product != null)
            {
                if (bag.Meals.Any(x => x.MealId == product.Id))
                {
                    var currentProduct = bag.Meals.FirstOrDefault(x => x.MealId == product.Id);
                    currentProduct.Quantity += quantity;

                    var log = new log_19118066
                    {
                        Id = Guid.NewGuid(),
                        TableName = "SelectedProduct",
                        CommandType = "Update",
                        ExecutionDate = DateTime.Now
                    };

                    await _context.log_19118066.AddAsync(log);

                    message = $"Успешно прибавихте още {quantity} порции {product.MealName}";
                }
                else
                {
                    var addToBag = new SelectedProduct
                    {
                        Id = Guid.NewGuid(),
                        Meal = product,
                        MealId = product.Id,
                        Bag = bag,
                        BagId = bag.Id,
                        Quantity = quantity,
                        LastModified_19118066 = DateTime.Now,
                    };

                    var log = new log_19118066
                    {
                        Id = Guid.NewGuid(),
                        TableName = "SelectedProduct",
                        CommandType = "Insert",
                        ExecutionDate = DateTime.Now
                    };

                    await _context.log_19118066.AddAsync(log);

                    bag.Meals.Add(addToBag);

                    var log_bag = new log_19118066
                    {
                        Id = Guid.NewGuid(),
                        TableName = "Bags",
                        CommandType = "Update",
                        ExecutionDate = DateTime.Now
                    };

                    await _context.log_19118066.AddAsync(log_bag);

                    message = $"Успешно добавихте {quantity} порции {product.MealName}";
                }

                await _context.SaveChangesAsync();

                return new JsonResult(message);
            }
            message = "Възникна грешка, моля опитайте отново!";
            return new JsonResult(message);
        }

        public async Task<Bag> CreateBag(string userId)
        {
            var bag = new Bag
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Status = "A",
                Meals = new List<SelectedProduct>(),
                LastModified_19118066 = DateTime.Now,
            };

            await _context.Bags.AddAsync(bag);

            var log = new log_19118066
            {
                Id = Guid.NewGuid(),
                TableName = "Bags",
                CommandType = "Insert",
                ExecutionDate = DateTime.Now
            };

            await _context.log_19118066.AddAsync(log);


            await _context.SaveChangesAsync();

            return bag;
        }

        public async Task<IActionResult> OnGetCount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var bag = await _context.Bags
                .Include(x => x.User)
                .Include(x => x.Meals)
                .ThenInclude(x => x.Meal)
                .FirstOrDefaultAsync(x => x.UserId == userId && x.Status == "A");

            if (bag == null)
            {
                return new JsonResult("0");
            }

            return new JsonResult(bag.Meals.Count);
        }

        public async Task<IActionResult> OnGetRemove(string id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var bag = await _context.Bags
                .Include(x => x.User)
                .Include(x => x.Meals)
                .ThenInclude(x => x.Meal)
                .FirstOrDefaultAsync(x => x.UserId == userId && x.Status == "A");

            if (bag != null && bag.Meals.Any(x => x.MealId == Guid.Parse(id)))
            {
                var product = bag.Meals.FirstOrDefault(x => x.MealId == Guid.Parse(id));

                if (product != null)
                {
                    bag.Meals.Remove(product);

                    var log = new log_19118066
                    {
                        Id = Guid.NewGuid(),
                        TableName = "Bags",
                        CommandType = "Update",
                        ExecutionDate = DateTime.Now
                    };

                    await _context.log_19118066.AddAsync(log);

                    var log_selectedProduct = new log_19118066
                    {
                        Id = Guid.NewGuid(),
                        TableName = "SelectedProduct",
                        CommandType = "Update",
                        ExecutionDate = DateTime.Now
                    };

                    await _context.log_19118066.AddAsync(log_selectedProduct);

                    await _context.SaveChangesAsync();
                }
            }

            return new JsonResult(bag.Meals.Sum(x => x.Quantity * x.Meal.MealPrice));
        }

        public async Task<IActionResult> OnGetChangeProductCount(Guid pId, int newCount)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var bag = await _context.Bags
                .Include(x => x.User)
                .Include(x => x.Meals)
                .ThenInclude(x => x.Meal)
                .FirstOrDefaultAsync(x => x.UserId == userId && x.Status == "A");

            if (bag != null && bag.Meals.Any(x => x.MealId == pId))
            {
                var product = bag.Meals.FirstOrDefault(x => x.MealId == pId);

                if (product != null)
                {
                    product.Quantity = newCount;

                    var log = new log_19118066
                    {
                        Id = Guid.NewGuid(),
                        TableName = "Bags",
                        CommandType = "Update",
                        ExecutionDate = DateTime.Now
                    };

                    await _context.log_19118066.AddAsync(log);

                    var log_selectedProduct = new log_19118066
                    {
                        Id = Guid.NewGuid(),
                        TableName = "SelectedProduct",
                        CommandType = "Update",
                        ExecutionDate = DateTime.Now
                    };

                    await _context.log_19118066.AddAsync(log_selectedProduct);

                    await _context.SaveChangesAsync();
                }
            }

            return new JsonResult(bag.Meals.Sum(x => x.Quantity * x.Meal.MealPrice));
        }
    }
}

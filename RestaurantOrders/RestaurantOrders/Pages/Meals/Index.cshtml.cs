using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RestaurantOrders.Data;

namespace RestaurantOrders.Pages.Meals
{
    public class IndexModel : PageModel
    {
        private readonly RestaurantOrders.Data.ApplicationDbContext _context;

        public IndexModel(RestaurantOrders.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Meal> Meal { get;set; } = default!;

        public Filter Filter { get; set; }

        public string Search { get; set; }

        public Sort Sort { get; set; }

        public int Page { get; set; } = 1;

        public int MealsCount = 6;

        public int LastPage { get; set; }

        public string Type { get; set; }

        public async Task OnGetAsync(Filter filter, Sort sort, int currPage, string search, string type)
        {
            if (_context.Meals != null)
            {
                Meal = await _context.Meals.Include(x=> x.Category).ToListAsync();
            }

            if(filter == Filter.Салати)
            {
                Meal = Meal.Where(x=> x.Category.Name == "Салати").ToList();
            }
            else if (filter == Filter.Предястия)
            {
                Meal = Meal.Where(x => x.Category.Name == "Предястия").ToList();
            }
            else if (filter == Filter.Основни)
            {
                Meal = Meal.Where(x => x.Category.Name == "Основни").ToList();
            }
            else if (filter == Filter.Десерти)
            {
                Meal = Meal.Where(x => x.Category.Name == "Десерти").ToList();

            }
            else if (filter == Filter.Напитки)
            {
                Meal = Meal.Where(x => x.Category.Name == "Напитки").ToList();
            }
            else if (filter == Filter.Алкохол)
            {
                Meal = Meal.Where(x => x.Category.Name == "Алкохол").ToList();
            }

            if(search != null)
            {
                Meal = Meal.Where(x => x.MealName.Contains(search, StringComparison.OrdinalIgnoreCase) || x.MealDescription.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if(type == null && type != "all")
            {
                Meal = Meal.Where(x => x.IsInMenu == true).ToList();
            }
            

            if (sort == Sort.PriceAsc)
            {
                Meal = Meal.OrderBy(x => x.MealPrice).ToList();
            }
            else if (sort == Sort.PriceDesc)
            {
                Meal = Meal.OrderByDescending(x => x.MealPrice).ToList();
            }
            else
            {
                Meal = Meal.OrderByDescending(x=> x.LasLastModified_19118066).ToList();
            }

            var lastPage = (int)Math.Ceiling((double)Meal.Count / MealsCount);

            if (currPage > lastPage)
            {
                if (lastPage == 0)
                {
                    lastPage = 1;
                }
                currPage = lastPage;
            }
            if (currPage == 0)
            {
                currPage = 1;
            }

            LastPage = lastPage;
            Page = currPage;
            Sort = sort;
            Filter = filter;
            Search = search;
            Type = type;
            Meal = Meal.Skip((Page - 1) * MealsCount)
           .Take(MealsCount).ToList();
        }
    }
}

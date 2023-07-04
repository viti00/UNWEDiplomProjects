using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PartsShop.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PartsShop.Pages.Parts
{
    public class IndexModel : PageModel
    {
        private readonly PartsShop.Data.ApplicationDbContext _context;

        public IndexModel(PartsShop.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Part> Part { get;set; } = default!;

        public int CurrentPage { get; set; } = 1;

        public int TotalPages { get; set; }

        public int PartsPerPage { get; set; } = 9;

        public string Search { get; set; }

        public CategoryFilter Filter { get; set; }

        public Sort Sort { get; set; }

        public async Task OnGetAsync(int currentPage, string search, CategoryFilter filter, Sort sort)
        {
            if (_context.Parts != null)
            {
                Part = await _context.Parts
                .Include(p => p.Category)
                .ToListAsync();
            }

            if(filter != CategoryFilter.All)
            {
                Part = FilterParts(Part.ToList(), filter);
            }
            if(search != null)
            {
                Part = Part.Where(x => x.PartName.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            if(sort != Sort.Normal)
            {
                Part = SortParts(Part.ToList(), sort);
            }

            var totalPages = (int)Math.Ceiling((double)Part.Count / PartsPerPage);

            if (currentPage > totalPages)
            {
                if (totalPages == 0)
                {
                    totalPages = 1;
                }
                currentPage = totalPages;
            }
            if (currentPage == 0)
            {
                currentPage = 1;
            }

            TotalPages = totalPages;
            CurrentPage = currentPage;
            Sort = sort;
            Filter = filter;
            Search = search;
            Part = Part.Skip((CurrentPage - 1) * PartsPerPage)
           .Take(PartsPerPage).ToList();
        }



        private List<Part> FilterParts(List<Part> parts, CategoryFilter filter)
        {
            switch (filter)
            {
                case CategoryFilter.BreakeSystem:
                    parts = parts.Where(x => x.Category.CategoryName == "Спирачна система").ToList();
                    break;
                case CategoryFilter.Suspension:
                    parts = parts.Where(x => x.Category.CategoryName == "Окачване").ToList();
                    break;
                case CategoryFilter.SteeringSystem:
                    parts = parts.Where(x => x.Category.CategoryName == "Кормилна система").ToList();
                    break;
                case CategoryFilter.EngineParts:
                    parts = parts.Where(x => x.Category.CategoryName == "Части за двигател").ToList();
                    break;
                case CategoryFilter.Sensors:
                    parts = parts.Where(x => x.Category.CategoryName == "Датчици").ToList();
                    break;
                case CategoryFilter.FuelSystem:
                    parts = parts.Where(x => x.Category.CategoryName == "Горивна система").ToList();
                    break;
                case CategoryFilter.MufflersAndPots:
                    parts = parts.Where(x => x.Category.CategoryName == "Ауспуси и гърнета").ToList();
                    break;
                case CategoryFilter.StartSystem:
                    parts = parts.Where(x => x.Category.CategoryName == "Стартова система").ToList();
                    break;
                case CategoryFilter.Lights:
                    parts = parts.Where(x => x.Category.CategoryName == "Светлини").ToList();
                    break;
            }

            return parts;
        }

        private List<Part> SortParts(List<Part> parts, Sort sort)
        {
            switch (sort)
            {
                case Sort.PriceAsc:
                    parts = parts.OrderBy(x => x.PartPrice).ToList();
                    break;
                case Sort.PriceDesc:
                    parts = parts.OrderByDescending(x => x.PartPrice).ToList();
                    break;
                case Sort.LastModifiedAsc:
                    parts = parts.OrderByDescending(x=> x.LastModified_19118073).ToList();
                    break;
                case Sort.LastModifiedDesc:
                    parts = parts.OrderBy(x => x.LastModified_19118073).ToList();
                    break;
            }

            return parts;
        }
    }
}

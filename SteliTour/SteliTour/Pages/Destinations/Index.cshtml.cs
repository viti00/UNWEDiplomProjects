using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SteliTour.Data;
using Stripe;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SteliTour.Pages.Destinations
{
    public class IndexModel : PageModel
    {
        private readonly SteliTour.Data.ApplicationDbContext _context;

        public IndexModel(SteliTour.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Destination> Destination { get;set; } = default!;

        public int CurrentPage { get; set; } = 1;

        public int EndPage { get; set; }

        public int ElementsPerPage = 9;

        public string Search { get; set; }

        public SortDestinations Sort { get; set; }

        public async Task OnGetAsync(int currPage, string search, SortDestinations sort)
        {
            if (_context.Destination != null)
            {
                Destination = await _context.Destination.Where(x=> x.Status == "A").ToListAsync();
            }

            if(search != null)
            {
                Destination = Destination.Where(x => x.Name.Contains(search, StringComparison.OrdinalIgnoreCase) || x.Description.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (sort == SortDestinations.PriceAdultAsc)
            {
                Destination = Destination.OrderBy(x => x.AdultPrice).ToList();
            }
            else if(sort == SortDestinations.PriceAdultDesc)
            {
                Destination = Destination.OrderByDescending(x => x.AdultPrice).ToList();
            }
            else if (sort == SortDestinations.PriceKidsAsc)
            {
                Destination = Destination.OrderBy(x => x.KidsPrice).ToList();
            }
            else if (sort == SortDestinations.PriceKidsDesc)
            {
                Destination = Destination.OrderByDescending(x => x.KidsPrice).ToList();
            }
            else if (sort == SortDestinations.PriceUnder2Asc)
            {
                Destination = Destination.OrderBy(x => x.Under2YearsKidsPrice).ToList();
            }
            else if (sort == SortDestinations.PriceUnder2Desc)
            {
                Destination = Destination.OrderByDescending(x => x.Under2YearsKidsPrice).ToList();
            }
            else if (sort == SortDestinations.DateAsc)
            {
                Destination = Destination.OrderBy(x => x.DateOfPublish).ToList();
            }
            else if (sort == SortDestinations.DateDesc)
            {
                Destination = Destination.OrderByDescending(x => x.DateOfPublish).ToList();
            }

            var lastPage = (int)Math.Ceiling((double)Destination.Count / ElementsPerPage);

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
            EndPage = lastPage;
            Sort = sort;
            CurrentPage = currPage;
            Search = search;
            Destination = Destination.Skip((CurrentPage - 1) * ElementsPerPage)
           .Take(ElementsPerPage).ToList();

        }
    }
}

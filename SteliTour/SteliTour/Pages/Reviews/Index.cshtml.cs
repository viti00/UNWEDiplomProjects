using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SteliTour.Data;
using SteliTour.Pages.Destinations;
using SteliTour.Pages.Requests;

namespace SteliTour.Pages.Reviews
{
    public class IndexModel : PageModel
    {
        private readonly SteliTour.Data.ApplicationDbContext _context;

        public IndexModel(SteliTour.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Review> Review { get;set; } = default!;

        public int CurrentPage { get; set; } = 1;

        public int EndPage { get; set; }

        public int ElementsPerPage = 9;

        public SortReviews Sort { get; set; }

        public async Task OnGetAsync(string type, int currPage, SortReviews sort)
        {
            if (_context.Review != null && type != null && type == "my")
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                Review = await _context.Review
               .Include(r => r.User).Where(x => x.UserId == userId).ToListAsync();
            }
            else if (_context.Review != null)
            {
                Review = await _context.Review
                .Include(r => r.User).ToListAsync();
            }

            if (sort == SortReviews.RatingAsc)
            {
                Review = Review.OrderBy(x => x.Rating).ToList();
            }
            else if (sort == SortReviews.RatingDesc)
            {
                Review = Review.OrderByDescending(x => x.Rating).ToList();
            }
            else if (sort == SortReviews.DateAsc)
            {
                Review = Review.OrderBy(x => x.ReviewDate).ToList();
            }
            else if (sort == SortReviews.DateDesc)
            {
                Review = Review.OrderByDescending(x => x.ReviewDate).ToList();
            }

            var lastPage = (int)Math.Ceiling((double)Review.Count / ElementsPerPage);

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
            Review = Review.Skip((CurrentPage - 1) * ElementsPerPage)
           .Take(ElementsPerPage).ToList();
        }
    }
}

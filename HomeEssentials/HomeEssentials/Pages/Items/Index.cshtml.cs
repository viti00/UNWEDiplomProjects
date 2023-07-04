using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HomeEssentials.Data;

namespace HomeEssentials.Pages.Items
{
    public class IndexModel : PageModel
    {
        private readonly HomeEssentials.Data.ApplicationDbContext _context;

        public IndexModel(HomeEssentials.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Item> Item { get;set; } = default!;

        public Filter Filter { get; set; }

        public string Search { get; set; }

        public Sort Sort { get; set; }

        public int PageIndex { get; set; } = 1;

        public int ItemsCount = 15;

        public int TotalPages { get; set; }



        public async Task OnGetAsync(Filter filter, Sort sort, string search,int pageIndex)
        {
            if (_context.Items != null)
            {
                Item = await _context.Items
                .Include(i => i.Category).Where(x=> x.IsActive == true).ToListAsync();

                if(filter != Filter.all)
                {
                    FilterItems(filter);
                }

                if(search != null)
                {
                    Item = Item.Where(x => x.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                if(sort != Sort.Normal)
                {
                    SortItems(sort);
                }

                var totalPages = (int)Math.Ceiling((double)Item.Count / ItemsCount);

                if (pageIndex > totalPages)
                {
                    if (totalPages == 0)
                    {
                        totalPages = 1;
                    }
                    pageIndex = totalPages;
                }
                if (pageIndex == 0)
                {
                    pageIndex = 1;
                }

                TotalPages = totalPages;
                PageIndex = pageIndex;
                Sort = sort;
                Filter = filter;
                Search = search;
                Item = Item.Skip((PageIndex - 1) * ItemsCount)
               .Take(ItemsCount).ToList();
            }
        }

        public void FilterItems(Filter filter)
        {
           
            string category = "";
            if(filter == Filter.tv)
            {
                category = "ТВ, Аудио и Електроника";
            }
            else if (filter == Filter.phone)
            {
                category = "Телефони и Таблети";
            }
            else if (filter == Filter.pc)
            {
                category = "Лаптопи, компютри и периферия";

            }
            else if (filter == Filter.dom)
            {
                category = "Домакински Електроуреди";
            }
            else if (filter == Filter.hb)
            {
                category = "Уреди за здраве и красота";
            }
            else if (filter == Filter.clima)
            {
                category = "Климатици, уреди за отопление и грижа за въздуха";
            }
            else if (filter == Filter.garden)
            {
                category = "Дом и Градина";
            }
            else if (filter == Filter.sport)
            {
                category = "Спорт и Свободно време";
            }
            else if (filter == Filter.photo)
            {
                category = "Фото и Видео";
            }
            else if (filter == Filter.kids)
            {
                category = "Играчки и Детски артикули";
            }

            Item = Item.Where(x=> x.Category.Category == category).ToList();
        }

        public void SortItems(Sort sort)
        {
            if(sort == Sort.PriceAscending)
            {
                Item.OrderBy(x => x.Price).ToList();
            }
            else if(sort == Sort.PriceDescending)
            {
                Item.OrderByDescending(x => x.Price);
            }
        }
    }
}

using System;

namespace CarPartsShop.Models.ViewModels
{
    public class QueryModelParts
    {
        public List<PartsAllViewModel> Parts { get; set; }

        public int PageElements { get; set; } = 9;

        public int CurrentPage { get; set; } = 0;

        public int LastPage { get; set; }

        public string Search { get; set; }

        public Sorting Sorting { get; set; }

        public Filter Filter { get; set; }
    }
}

namespace CarPartsShop.Models.ViewModels
{
    public class QueryModelOrders
    {
        public List<OrderViewModel> Orders { get; set; } = new List<OrderViewModel>();
        public string Search { get; set; }

        public SortOrders SortOrders { get; set; }

        public FilterOrders FilterOrders { get; set; }
    }
}

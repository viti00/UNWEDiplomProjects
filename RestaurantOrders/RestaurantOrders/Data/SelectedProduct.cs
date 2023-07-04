using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantOrders.Data
{
    [Table("SelectedProducts", Schema = "19118066")]
    public class SelectedProduct
    {
        public Guid Id { get; set; }

        [ForeignKey(nameof(Meal))]
        public Guid MealId { get; set; }

        public Meal Meal { get; set; }

        [ForeignKey(nameof(Bag))]
        public Guid BagId { get; set; }

        public Bag Bag { get; set; }

        public int Quantity { get; set; }

        public DateTime LastModified_19118066 { get; set; }
    }
}

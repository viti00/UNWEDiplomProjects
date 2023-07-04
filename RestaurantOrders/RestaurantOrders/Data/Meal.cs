using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantOrders.Data
{
    [Table("Meals", Schema = "19118066")]
    public class Meal
    {
        public Guid Id { get; set; }

        public string MealName { get; set; }
        public string MealDescription { get; set; }

        public double MealPrice { get; set; }

        public string MealImgUrl { get; set; }

        public DateTime? LasLastModified_19118066 { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public bool? IsInMenu { get; set; } = true;
    }
}

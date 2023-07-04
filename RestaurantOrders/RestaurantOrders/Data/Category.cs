using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantOrders.Data
{
    [Table("Categories", Schema = "19118066")]
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime LastModified_19118066 { get; set; }
    }
}

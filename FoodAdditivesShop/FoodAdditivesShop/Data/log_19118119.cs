using System.ComponentModel.DataAnnotations.Schema;

namespace FoodAdditivesShop.Data
{
    [Table("log_19118119", Schema = "19118119")]
    public class log_19118119
    {
        public Guid Id { get; set; }

        public string Table { get; set; }

        public string Action { get; set; }

        public DateTime ActionTime { get; set; }
    }
}

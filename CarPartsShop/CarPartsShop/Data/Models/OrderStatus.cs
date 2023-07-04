using System.ComponentModel.DataAnnotations.Schema;

namespace CarPartsShop.Data.Models
{
    [Table("OrderStatuses", Schema = "19118067")]
    public class OrderStatus
    {
        public int Id { get; set; } 

        public string Status { get; set; }

        public DateTime LastModified_19118067 { get; set; }
    }
}

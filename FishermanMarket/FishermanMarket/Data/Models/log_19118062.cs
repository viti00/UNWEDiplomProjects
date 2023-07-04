using System.ComponentModel.DataAnnotations.Schema;

namespace FishermanMarket.Data.Models
{
    [Table("log_19118062", Schema = "19118062")]
    public class log_19118062
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Table { get; set; }

        public string Operation { get; set; }

        public DateTime Time { get; set; }
    }
}

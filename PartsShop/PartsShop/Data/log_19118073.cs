using System.ComponentModel.DataAnnotations.Schema;

namespace PartsShop.Data
{
    [Table("log_19118073", Schema = "19118073")]
    public class log_19118073
    {
        public Guid Id { get; set; }

        public string Table { get; set; }

        public string Action { get; set; }

        public DateTime ActionTime { get; set; }
    }
}

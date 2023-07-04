using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantOrders.Data
{
    [Table("log_19118066", Schema = "19118066")]
    public class log_19118066
    {
        public Guid Id { get; set; }

        public string TableName { get; set; }

        public string CommandType { get; set; }

        public DateTime ExecutionDate { get; set; }
    }
}

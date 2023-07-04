using System.ComponentModel.DataAnnotations.Schema;

namespace CarPartsShop.Data.Models
{
    [Table("log_19118067", Schema = "19118067")]
    public class log_19118067
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string TableName { get; set; }

        public string OperationType { get; set; }

        public DateTime OperationCreateTime { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace VBoutique.Data
{
    [Table("log_19118155", Schema = "19118155")]
    public class log_19118155
    {
        public Guid Id { get; set; }

        public string TableName { get; set; }

        public string OperationType { get; set; }

        public DateTime OperationTime { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace HomeEssentials.Data
{
    [Table("OrderStatuses", Schema = "19118075")]
    public class OrderStatuses
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime? LastModified_19118075 { get; set; }
    }
}

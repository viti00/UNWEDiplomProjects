using System.ComponentModel.DataAnnotations.Schema;

namespace HomeEssentials.Data
{
    [Table("CartStatuses", Schema = "19118075")]
    public class CartStatus
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime? LastModified_19118075 { get; set; }
    }
}

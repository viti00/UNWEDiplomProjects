using System.ComponentModel.DataAnnotations.Schema;

namespace HomeEssentials.Data
{
    [Table("Categories", Schema = "19118075")]
    public class ItemCategory
    {
        public int Id { get; set; }

        public string Category { get; set; }

        public DateTime? LastModified_19118075 { get; set; }
    }
}

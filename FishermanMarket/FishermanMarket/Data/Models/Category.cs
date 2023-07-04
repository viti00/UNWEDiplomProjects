using System.ComponentModel.DataAnnotations.Schema;

namespace FishermanMarket.Data.Models
{
    [Table("Categories", Schema = "19118062")]
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime LastModified_19118062 { get; set; }
    }
}

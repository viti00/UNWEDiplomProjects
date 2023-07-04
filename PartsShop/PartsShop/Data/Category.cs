using System.ComponentModel.DataAnnotations.Schema;

namespace PartsShop.Data
{
    [Table("Categories", Schema = "19118073")]
    public class Category
    {
        public int Id { get; set; }

        public string CategoryName { get; set; }

        public DateTime? LastModified_19118073 { get; set; }

    }
}

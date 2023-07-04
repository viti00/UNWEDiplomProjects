using System.ComponentModel.DataAnnotations.Schema;

namespace CarPartsShop.Data.Models
{
    [Table("PartConditions", Schema = "19118067")]

    public class PartCondtion
    {
        public int Id { get; set; }

        public string Condition { get; set; }

        public DateTime LastModified_19118067 { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace CarPartsShop.Data.Models
{

    [Table("PartTypes", Schema = "19118067")]
    public class PartType
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public DateTime LastModified_19118067 { get; set; }
    }
}

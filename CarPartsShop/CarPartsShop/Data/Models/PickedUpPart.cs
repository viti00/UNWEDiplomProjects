using System.ComponentModel.DataAnnotations.Schema;

namespace CarPartsShop.Data.Models
{
    [Table("PickedUpParts", Schema = "19118067")]
    public class PickedUpPart
    {
        [ForeignKey(nameof(Cart))]
        public string CartId { get; set; }

        public Cart Cart { get; set; }

        [ForeignKey(nameof(Part))]
        public string PartId { get; set; }

        public Part Part { get; set; }

        public int Count { get; set; }

        public DateTime LastModified_19118067 { get; set; }
    }
}

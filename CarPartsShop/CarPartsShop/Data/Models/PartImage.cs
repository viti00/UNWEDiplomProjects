using System.ComponentModel.DataAnnotations.Schema;

namespace CarPartsShop.Data.Models
{
    [Table("PartImages", Schema = "19118067")]
    public class PartImage
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public byte[] Bytes { get; set; }

        public string Description { get; set; }

        public string FileExtension { get; set; }

        public double Size { get; set; }

        [ForeignKey(nameof(Part))]
        public string PartId { get; set; }

        public Part Part { get; set; }

        public DateTime LastModified_19118067 { get; set; }
    }
}

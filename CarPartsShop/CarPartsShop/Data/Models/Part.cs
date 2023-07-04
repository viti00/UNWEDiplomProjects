using System.ComponentModel.DataAnnotations.Schema;

namespace CarPartsShop.Data.Models
{
    [Table("Parts", Schema = "19118067")]
    public class Part
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; }

        public string Description { get; set; }

        public int StockQnt { get; set; }

        public double PartPrice { get; set; }

        [ForeignKey(nameof(PartType))]
        public int PartTypeId { get; set; }
        public PartType PartType { get; set; }

        [ForeignKey(nameof(PartCondtion))]
        public int PartConditionId { get; set; }

        public PartCondtion PartCondtion { get; set; }

        public List<PartImage> PartImages { get; set; } = new List<PartImage>();

        public List<PickedUpPart> PickedUpParts { get; set; } = new List<PickedUpPart>();

        public DateTime LastModified_19118067 { get; set; }
    }
}

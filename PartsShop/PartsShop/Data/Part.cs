using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace PartsShop.Data
{
    [Table("Parts", Schema = "19118073")]
    public class Part
    {
        public Guid Id { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public string PartName { get; set; }

        public string PartDescription { get; set; }

        public string ImageUrl { get; set; }

        public double PartPrice { get; set; }

        public List<CartPart>? CartParts { get; set; }

        public List<PartReview>? PartReviews { get; set; }

        public DateTime? DateOfAdd { get; set; }

        public DateTime? LastModified_19118073 { get; set; }

    }
}

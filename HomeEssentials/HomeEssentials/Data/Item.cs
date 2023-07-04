using System.ComponentModel.DataAnnotations.Schema;

namespace HomeEssentials.Data
{
    [Table("Items", Schema = "19118075")]
    public class Item
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public ItemCategory? Category { get; set; }

        public string ItemDescription { get; set; }

        public double Price { get; set; }

        public string ItemImageUrl { get; set; }

        public List<ItemInCart>? CartItems { get; set; }

        public List<Review>? Reviews { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? LastModified_19118075 { get; set; }

        public bool? IsActive { get; set; }
    }
}

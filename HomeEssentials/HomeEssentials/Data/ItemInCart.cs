using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeEssentials.Data
{
    [Table("ItemsInCart", Schema = "19118075")]
    public class ItemInCart
    {
        public Guid Id { get; set; }


        [ForeignKey("Item")]
        public Guid ItemId { get; set; }

        public Item Item { get; set; }

        [ForeignKey("Cart")]
        public Guid CartId { get; set; }
        public Cart Cart { get; set; }

        public int ItemCount { get; set; }
        public DateTime? LastModified_19118075 { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FishermanMarket.Data.Models
{
    [Table("Products", Schema = "19118062")]
    public class Product
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public Category? Category {get;set;}

        public List<BagProduct>? Products { get; set; }

        public ProductImage? Image { get; set; }

        [HiddenInput(DisplayValue = false)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime LastModified_19118062 { get; set; } = DateTime.Now;

        [NotMapped]
        [FromForm]
        public IFormFile? File { get; set; }
    }
}

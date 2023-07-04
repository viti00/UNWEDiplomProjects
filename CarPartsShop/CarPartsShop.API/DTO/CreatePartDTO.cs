using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarPartsShop.API.DTO
{
    public class CreatePartDTO
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int StockQnt { get; set; }

        public double PartPrice { get; set; }

        public int PartType { get; set; }

        public int PartCondition { get; set; }

        [FromForm]
        [NotMapped]
        public IFormFileCollection? Files { get; set; }
    }
}

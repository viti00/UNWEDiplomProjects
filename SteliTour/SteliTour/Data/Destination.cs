using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SteliTour.Data
{
    [Table("Destinations", Schema = "19118105")]
    public class Destination
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double AdultPrice { get; set; }

        public double KidsPrice { get; set; }

        public double Under2YearsKidsPrice { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        public DateTime DateOfPublish { get; set; }

        public DateTime LastModified_19118105 { get; set; }

        public string? Status { get; set; }

        public string ImageUrl { get; set; }

        public string HotelSiteUrl { get; set; }

        public string? DisplayName
        {
            get
            {
                return $"{Name} - {StartDate:dd.MM.yyyy} - {EndDate:dd.MM.yyyy}";
            }
        }
    }
}

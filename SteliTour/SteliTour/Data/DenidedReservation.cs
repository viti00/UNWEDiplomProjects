using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SteliTour.Data
{
    [Table("DenidedReservations", Schema = "19118105")]
    public class DenidedReservation
    {
        public Guid Id { get; set; }

        public string UserEmail { get; set; }

        public string Destination { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EGN { get; set; }

        public string PhoneNumber { get; set; }
        public int AdultCount { get; set; }

        public int KidsCount { get; set; }

        public int Under2YearsKidsCount { get; set; }

        public double? Payed { get; set; }

        public double? Remainign { get; set; }

        public string DateOfReservation { get; set; }
        public DateTime DateOfCancelation { get; set; }

        public DateTime LastModified_19118105 { get; set; }
    }
}

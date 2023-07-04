using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;

namespace SteliTour.Data
{
    [Table("Reservations", Schema = "19118105")]
    public class Reservation
    {
        public Guid Id { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public IdentityUser? User { get; set; }

        [ForeignKey(nameof(Destination))]
        public Guid DestinationId { get; set; }

        public Destination? Destination { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [RegularExpression("^[0-9]{10}$", ErrorMessage ="Невалидно ЕГН")]
        public string EGN { get; set; }

        public string PhoneNumber { get; set; }
        public int AdultCount { get; set; }

        public int KidsCount { get; set; }

        public int Under2YearsKidsCount { get; set; }

        public double? Payed { get; set; }

        public double? Remainign { get; set; }

        public DateTime? DateOfReservation { get; set; }

        public DateTime? LastModified_19118105 { get; set; }

        public string? Status { get; set; }
    }
}

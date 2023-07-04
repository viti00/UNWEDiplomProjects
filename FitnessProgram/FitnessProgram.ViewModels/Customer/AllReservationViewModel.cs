using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessProgram.ViewModels.Customer
{
    public class AllReservationViewModel
    {
        public string ReservationId { get; set; }
        public int RangeId { get; set; }

        public string? Name { get; set; }

        public string? Address { get; set; }

        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}

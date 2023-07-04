using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessProgram.ViewModels.Customer
{
    public class ReservationViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string Date { get; set; }

        public string TimeRange { get; set; }

        public DateTime LastAvailableForModification { get; set; }
    }
}

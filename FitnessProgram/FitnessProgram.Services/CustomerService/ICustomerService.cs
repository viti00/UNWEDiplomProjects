namespace FitnessProgram.Services.CustomerService
{
    using FitnessProgram.Data.Models;
    using FitnessProgram.ViewModels.Customer;

    public interface ICustomerService
    {
        public List<ReservationDateTimeRange> GetReservation(string id);

        public bool CreateReservation(ReservationFormModel model, string userId);

        public ReservationDateTimeRange GetCurrentTimeRange(string dateId, int rangeId);

        public List<ReservationViewModel> GetMyReservations(string userId);

        public bool EditReservation(string resId, string name, string address, string phone);

        public bool RejectReservation(string resId);

        public List<AllReservationViewModel> GetAllReservations(string dateOfReservationId);
        public Reservation GetReservationById(string resId);

        public ExportModel ExportToExcel();
    }
}

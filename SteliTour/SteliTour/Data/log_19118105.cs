using System.ComponentModel.DataAnnotations.Schema;

namespace SteliTour.Data
{
    [Table("log_19118105", Schema = "19118105")]
    public class log_19118105
    {
        public Guid Id { get; set; }

        public string Operation { get; set; }

        public string Table { get; set; }

        public DateTime Date { get; set; }
    }
}

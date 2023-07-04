



namespace FitnessProgram.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("log_19118074", Schema = "19118074")]
    public class log_19118074
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Table { get; set; }

        public string Operation { get; set; }

        public DateTime Time { get; set; }
    }
}

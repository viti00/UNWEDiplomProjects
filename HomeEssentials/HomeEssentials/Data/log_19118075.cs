using System.ComponentModel.DataAnnotations.Schema;

namespace HomeEssentials.Data
{
    [Table("log_19118075", Schema = "19118075")]
    public class log_19118075
    {
        public Guid Id { get; set; }

        public string Table { get; set; }

        public string Command { get; set; }

        public DateTime DateTime { get; set; }

    }
}

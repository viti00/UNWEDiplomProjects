using System.ComponentModel.DataAnnotations.Schema;

namespace VBoutique.Data
{
    [Table("Colors", Schema = "19118155")]
    public class Color
    {
        public int Id { get; set; }

        public string ColorNameBg { get; set; }
        public string ColorNameEn { get; set; }

        public DateTime? LastModified_19118155 { get; set; }
    }
}

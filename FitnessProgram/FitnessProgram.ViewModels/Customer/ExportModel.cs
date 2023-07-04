using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessProgram.ViewModels.Customer
{
    public class ExportModel
    {
        public MemoryStream Memory { get; set; }

        public string Url { get; set; } = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public string FileName { get; set; }
    }
}

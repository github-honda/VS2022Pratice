using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleWorkerService
{
    public class Config
    {
        public int Enable { get; set; }
        public string InputFolder { get; set; } = string.Empty;
        public string OutputFolder { get; set; } = string.Empty;
        public string BackupFolder { get; set; } = string.Empty;
    }
}

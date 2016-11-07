using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abraxas.Scheduler.Core.Model
{
    public class LogEntry
    {
        public string FileName { get; set; }
        public string FullName { get; set; }
        public DateTime LastModification { get; set; }
    }
}

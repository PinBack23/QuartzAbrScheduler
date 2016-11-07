using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abraxas.Scheduler.Core.Model
{
    public class Alert
    {
        public Job FailJob { get; set; }
        public string ErrorMessage { get; set; }
        public Exception Fail { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}

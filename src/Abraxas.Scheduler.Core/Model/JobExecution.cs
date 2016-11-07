using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abraxas.Scheduler.Core.Model
{
    public class JobExecution
    {
        public Job Job { get; set; }
        public bool Successfully { get; set; }
        public object Result { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abraxas.Scheduler.Core.Model
{
    public class Trigger
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public string TypeName { get; set; }
        public string State { get; set; }
        public DateTime? NextFireTime { get; set; }
        public DateTime? PreviousFireTime { get; set; }
        public string CronExpression { get; set; }
    }
}

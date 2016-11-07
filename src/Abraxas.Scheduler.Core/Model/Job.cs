using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abraxas.Scheduler.Core.Model
{
    public class Job
    {
        public Job()
        {
            this.TriggerList = new List<Trigger>();
        }

        public string Name { get; set; }
        public string Group { get; set; }
        public string Description { get; set; }
        public string TypeName { get; set; }
        public string AssemblyName { get; set; }
        public List<Trigger> TriggerList { get; set; }
    }
}

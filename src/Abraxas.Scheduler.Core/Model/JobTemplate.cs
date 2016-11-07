using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abraxas.Scheduler.Core.Model
{
    public class JobTemplate
    {
        public JobTemplate()
        {
            this.MandatoryConfigFields = new List<string>();
            this.OptionalConfigFields = new List<string>();
        }

        public string JobType { get; set; }
        public string Description { get; set; }
        public string DefaultName { get; set; }
        public string DefaultGroup { get; set; }
        public List<string> MandatoryConfigFields { get; set; }
        public List<string> OptionalConfigFields { get; set; }

    }
}

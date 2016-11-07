using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abraxas.Scheduler.Core.Model
{
    public class JobRelation
    {
        public string JobName { get; set; }
        public string Group { get; set; }
        public string CronExpression { get; set; }
        public string Type { get; set; }
        public string JsonConfig { get; set; }

        public override int GetHashCode()
        {
            return this.JobName.EmptyOrValue().GetHashCode() ^
                this.Group.EmptyOrValue().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            JobRelation loCheck = obj as JobRelation;
            if (loCheck == null)
                return false;
            return this.JobName == loCheck.JobName &&
                this.Group == loCheck.Group;
        }
    }
}

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Abraxas.Scheduler.Core.Model
//{
//    public class TriggerRelation
//    {
//        public string JobName { get; set; }
//        public string Group { get; set; }
//        public string TriggerName { get; set; }
//        public string CronExpression { get; set; }

//        public override int GetHashCode()
//        {
//            return this.JobName.EmptyOrValue().GetHashCode() ^
//                this.Group.EmptyOrValue().GetHashCode() ^
//                this.TriggerName.EmptyOrValue().GetHashCode();
//        }

//        public override bool Equals(object obj)
//        {
//            TriggerRelation loCheck = obj as TriggerRelation;
//            if (loCheck == null)
//                return false;
//            return this.JobName == loCheck.JobName &&
//                this.Group == loCheck.Group &&
//                this.TriggerName == loCheck.TriggerName;
//        }
//    }
//}

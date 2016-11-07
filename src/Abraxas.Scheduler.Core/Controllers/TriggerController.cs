using Abraxas.Scheduler.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Abraxas.Scheduler.Core.Controllers
{
    public class TriggerController : ApiController
    {
        public int GetCount()
        {
            return Scheduler.Instance.TriggerCount();
        }

        public IEnumerable<Trigger> GetTriggers()
        {
            return Scheduler.Instance.GetTrigger();
        }

        public void PutPause(Trigger poTrigger)
        {
            Scheduler.Instance.PauseTrigger(poTrigger);
        }

        public void PutToggle(Trigger poTrigger)
        {
            Scheduler.Instance.ToggleTrigger(poTrigger);
        }

        //public void PutAddRelation(TriggerRelation poTriggerRelation)
        //{
        //    Scheduler.Instance.AddRelation(poTriggerRelation);
        //}

    }
}

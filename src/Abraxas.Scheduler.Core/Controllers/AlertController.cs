using Abraxas.Scheduler.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Abraxas.Scheduler.Core.Controllers
{
    public class AlertController : ApiController
    {

        public IEnumerable<Alert> GetAlerts(int pnTake = 0)
        {
            if (pnTake == 0)
                return AlertContainer.Instance.ErrorContainer;
            else
                return AlertContainer.Instance.ErrorContainer.OrderByDescending(item => item.TimeStamp).Take(pnTake);
        }
    }
}

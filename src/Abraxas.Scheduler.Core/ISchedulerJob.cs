using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abraxas.Scheduler.Core
{
    public interface ISchedulerJob : IJob
    {
        string DefaultName { get; }
        string DefaultGroup { get; }
        string Description { get; }

        List<string> MandatoryConfigFields { get; }
        List<string> OptionalConfigFields { get; }
    }
}

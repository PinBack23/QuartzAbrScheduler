using Abraxas.Scheduler.Core.Model;
using Common.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abraxas.Scheduler.Core
{
    internal class JobListener : IJobListener
    {
        private static readonly ILog moLogger = LogManager.GetLogger(Constants.StdLogger);

        #region IJobListener Member

        public void JobExecutionVetoed(IJobExecutionContext context)
        {
            
        }

        public void JobToBeExecuted(IJobExecutionContext context)
        {
            
        }

        public void JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException)
        {
            moLogger.Debug("Job was executed.");
            moLogger.DebugFormat("Job-Name: {0}.", context.JobDetail.Key.Name);
            moLogger.DebugFormat("Job-Group: {0}.", context.JobDetail.Key.Group);
            moLogger.InfoFormat("NextFireTime for job: {0}.", TimeZone.CurrentTimeZone.ToLocalTime(context.NextFireTimeUtc.GetValueOrDefault().DateTime));
            moLogger.DebugFormat("PreviousFireTime for job: {0}.", TimeZone.CurrentTimeZone.ToLocalTime(context.PreviousFireTimeUtc.GetValueOrDefault().DateTime));
            Job loJob = new Job()
            {
                Name = context.JobDetail.Key.Name,
                Group = context.JobDetail.Key.Group,
                Description = context.JobDetail.Description,
                TypeName = context.JobDetail.JobType.FullName,
                AssemblyName = Path.GetFileName(context.JobDetail.JobType.Assembly.Location)
            };

            if (jobException != null)
            {
                moLogger.Error("Job execution failed.", jobException);
                AlertContainer.Instance.Add(loJob, jobException);
            }

            JobExecutionContainer.Instance.Add(new JobExecution()
            {
                Job = loJob,
                Successfully = jobException == null,
                TimeStamp = DateTime.Now,
                Result = context.Result
            });

        }

        public string Name
        {
            get { return "Abraxas.Scheduler.Core.JobListener"; }
        }

        #endregion
    }
}

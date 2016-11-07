using Abraxas.Scheduler.Core;
using Common.Logging;
using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abraxas.Scheduler.Jobs
{
    public class LogJob : ISchedulerJob
    {
        private static readonly ILog moLogger = LogManager.GetLogger(Constants.StdLogger);

        public class Config
        {
            public string Loggername { get; set; }
            public string LogPrefix { get; set; }
        }

        private List<string> moMandatoryConfigFields = new List<string>();
        private List<string> moOptionalConfigFields = new List<string>();

        public LogJob()
        {
            this.moMandatoryConfigFields.Add("LogPrefix");
            this.OptionalConfigFields.Add("Loggername");
        }

        public string DefaultName
        {
            get { return "Logger Job"; }
        }

        public string DefaultGroup
        {
            get { return "Logging"; }
        }

        public string Description
        {
            get { return "Ich logge einfach was"; }
        }

        public List<string> MandatoryConfigFields { get { return this.moMandatoryConfigFields; } }

        public List<string> OptionalConfigFields { get { return this.moOptionalConfigFields; } }

        public void Execute(IJobExecutionContext context)
        {
            var loConfig = JsonConvert.DeserializeObject<Config>(context.JobDetail.JobDataMap.GetString(Constants.JobConfigEntry));
            this.RaiseException(loConfig);

            if (loConfig.Loggername.IsNotEmpty())
                LogManager.GetLogger(loConfig.Loggername).InfoFormat("{0}: {1:G}", loConfig.LogPrefix, DateTime.Now);    
            else
                moLogger.InfoFormat("{0}: {1:G}", loConfig.LogPrefix, DateTime.Now);    
        }

        private void RaiseException(Config poConfig)
        {
            if (poConfig.LogPrefix.ToUpper() == "ERROR")
                throw new ArgumentException("Error in Config", "Config ");
        }
       
    }
}

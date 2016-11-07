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
    public class DeleteJob : ISchedulerJob
    {
        public class Config
        {
            public string DirectoryFullPath { get; set; }
        }

        private static readonly ILog moLogger = LogManager.GetLogger(Constants.StdLogger);

        private List<string> moMandatoryConfigFields = new List<string>();
        private List<string> moOptionalConfigFields = new List<string>();

        public DeleteJob()
        {
            this.moMandatoryConfigFields.Add("DirectoryFullPath");
        }

        public string DefaultName
        {
            get { return "Delete Directory"; }
        }

        public string DefaultGroup
        {
            get { return "Destroying"; }
        }

        public string Description
        {
            get { return "Ich lösche ein Verzeichnis"; }
        }

        public void Execute(IJobExecutionContext context)
        {
            var loConfig = JsonConvert.DeserializeObject<Config>(context.JobDetail.JobDataMap.GetString(Constants.JobConfigEntry));

            moLogger.InfoFormat("Ich lösche das Verzeichnis {0}", loConfig.DirectoryFullPath);
            //System.IO.Directory.Delete(@"E:\Temp\delete", true);
        }

        public List<string> MandatoryConfigFields { get { return this.moMandatoryConfigFields; } }

        public List<string> OptionalConfigFields { get { return this.moOptionalConfigFields; } }
    }
}

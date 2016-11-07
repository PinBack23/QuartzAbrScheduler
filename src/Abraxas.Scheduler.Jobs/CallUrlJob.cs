using Abraxas.Scheduler.Core;
using Common.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Abraxas.Scheduler.Jobs
{
    public class CallUrlJob : ISchedulerJob
    {
        private static readonly ILog moLogger = LogManager.GetLogger(Constants.StdLogger);

        public class Config
        {
            public string Url { get; set; }
        }

        private List<string> moMandatoryConfigFields = new List<string>();
        private List<string> moOptionalConfigFields = new List<string>();

        public CallUrlJob()
        {
            this.moMandatoryConfigFields.Add("Url");
        }

        public string DefaultName
        {
            get { return "Call Url"; }
        }

        public string DefaultGroup
        {
            get { return "Web"; }
        }

        public string Description
        {
            get { return "Call and Url and log response"; }
        }

        public List<string> MandatoryConfigFields { get { return this.moMandatoryConfigFields; } }

        public List<string> OptionalConfigFields { get { return this.moOptionalConfigFields; } }

        public void Execute(Quartz.IJobExecutionContext context)
        {
            var loConfig = JsonConvert.DeserializeObject<Config>(context.JobDetail.JobDataMap.GetString(Constants.JobConfigEntry));
            this.RaiseException(loConfig);

            string lsResponse;
            WebClient loNewWebClient = new WebClient();
            lsResponse = loNewWebClient.DownloadString(loConfig.Url);

            //WebRequest loRequest = HttpWebRequest.Create(loConfig.Url);
            //WebResponse loResponse = loRequest.GetResponse();

            //using (StreamReader loReader = new StreamReader(loResponse.GetResponseStream()))
            //{
            //    lsResponse = loReader.ReadToEnd();
            //}

            moLogger.InfoFormat("Call url {0} successfully", loConfig.Url);
            context.Result = lsResponse;
        }

        private void RaiseException(Config poConfig)
        {
            if (poConfig.Url.ToUpper() == "ERROR")
                throw new ArgumentException("Error in Config", "Config ");
        }
    }
}

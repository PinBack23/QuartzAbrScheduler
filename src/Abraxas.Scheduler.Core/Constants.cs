using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abraxas.Scheduler.Core
{
    public static class Constants
    {
        public static readonly string StdLogger = "SchedulerLogger";
        public static readonly string JobConfigEntry = "JobConfig";


        public static string JobDirectory
        {
            get
            {
                return CreateDirectoryIfNecessary(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Jobs"));
            }
        }

        //public static string TriggerDirectory
        //{
        //    get
        //    {
        //        return CreateDirectoryIfNecessary(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Trigger"));
        //    }
        //}

        public static string PersistenceDirectory
        {
            get
            {
                return CreateDirectoryIfNecessary(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Persistence"));
            }
        }

        public static string UploadDirectory
        {
            get
            {
                return CreateDirectoryIfNecessary(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Upload"));
            }
        }

        public static string LogDirectory
        {
            get
            {
                return CreateDirectoryIfNecessary(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log"));
            }
        }

        private static string CreateDirectoryIfNecessary(string psDirectory)
        {
            if (!Directory.Exists(psDirectory))
                Directory.CreateDirectory(psDirectory);
            return psDirectory;
        }
    }
}

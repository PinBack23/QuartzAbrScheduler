using Abraxas.Scheduler.Core.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abraxas.Scheduler.Core.Persistence
{
    public class JobRepository
    {

        #region Singleton

        /// <summary>
        /// Constructor legt Singleton-Instanz an und kann nur einmal aufgerufen werden.
        /// </summary>    
        protected JobRepository() { }

        /// <summary>
        /// Instance Property
        /// </summary>
        public static JobRepository Instance
        {
            get { return NestedSingleton.Instance; }
        }

        private class NestedSingleton
        {
            internal static readonly JobRepository Instance = new JobRepository();

            // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
            // NICHT ENTFERNEN AUCH WENN FXCOP WAS ANDERES SAGT
            static NestedSingleton() { }
        }
        #endregion Singleton

        public List<JobRelation> Read()
        {
            FileInfo loFileInfo = new FileInfo(this.JobFile);
            List<JobRelation> loList = new List<JobRelation>();
            if (loFileInfo.Exists)
            {
                try
                {
                    loList = JsonConvert.DeserializeObject<List<JobRelation>>(File.ReadAllText(loFileInfo.FullName));
                }
                catch
                {
                    loList = new List<JobRelation>();
                }
            }
            return loList;
        }

        public void Write(List<JobRelation> poList)
        {
            if (poList != null)
                File.WriteAllText(this.JobFile, JsonConvert.SerializeObject(poList));
        }

        public  void Write(JobRelation poJobRelation)
        {
            var loList = this.Read();
            loList.Remove(poJobRelation);
            loList.Add(poJobRelation);
            this.Write(loList);
        }

        public void Delete(JobRelation poJobRelation)
        {
            var loList = this.Read();
            loList.Remove(poJobRelation);
            this.Write(loList);
        }

        private string JobFile
        {
            get { return Path.Combine(Constants.PersistenceDirectory, "jobs.dat"); }
        }
    }
}

//using Abraxas.Scheduler.Core.Model;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Abraxas.Scheduler.Core.Persistence
//{
//    public class TriggerRepository
//    {

//        #region Singleton

//        /// <summary>
//        /// Constructor legt Singleton-Instanz an und kann nur einmal aufgerufen werden.
//        /// </summary>    
//        protected TriggerRepository() { }

//        /// <summary>
//        /// Instance Property
//        /// </summary>
//        public static TriggerRepository Instance
//        {
//            get { return NestedSingleton.Instance; }
//        }

//        private class NestedSingleton
//        {
//            internal static readonly TriggerRepository Instance = new TriggerRepository();

//            // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
//            // NICHT ENTFERNEN AUCH WENN FXCOP WAS ANDERES SAGT
//            static NestedSingleton() { }
//        }
//        #endregion Singleton

//        public List<TriggerRelation> Read()
//        {
//            FileInfo loFileInfo = new FileInfo(this.TriggerFile);
//            List<TriggerRelation> loList = new List<TriggerRelation>();
//            if (loFileInfo.Exists)
//            {
//                try
//                {
//                    loList = JsonConvert.DeserializeObject<List<TriggerRelation>>(File.ReadAllText(loFileInfo.FullName));
//                }
//                catch
//                {
//                    loList = new List<TriggerRelation>();
//                }
//            }
//            return loList;
//        }

//        public void Write(List<TriggerRelation> poList)
//        {
//            if (poList != null)
//                File.WriteAllText(this.TriggerFile, JsonConvert.SerializeObject(poList));
//        }

//        private string TriggerFile
//        {
//            get { return Path.Combine(Constants.TriggerDirectory, "trigger.dat"); }
//        }
//    }
//}

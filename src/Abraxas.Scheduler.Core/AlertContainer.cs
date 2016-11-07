using Abraxas.Scheduler.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abraxas.Scheduler.Core
{
    public class AlertContainer
    {
        private List<Alert> moErrorContainer = new List<Alert>();

        #region Singleton

        /// <summary>
        /// Constructor legt Singleton-Instanz an und kann nur einmal aufgerufen werden.
        /// </summary>    
        protected AlertContainer() { }

        /// <summary>
        /// Instance Property
        /// </summary>
        public static AlertContainer Instance
        {
            get { return NestedSingleton.Instance; }
        }

        private class NestedSingleton
        {
            internal static readonly AlertContainer Instance = new AlertContainer();

            // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
            // NICHT ENTFERNEN AUCH WENN FXCOP WAS ANDERES SAGT
            static NestedSingleton() { }
        }
        #endregion Singleton

        public void Add(Job poJob, Exception poException)
        {
            this.moErrorContainer.Add(new Alert()
                {
                    FailJob = poJob,
                    Fail = poException,
                    ErrorMessage= this.LastErrorMessage(poException),
                    TimeStamp = DateTime.Now
                });
        }

        public List<Alert> ErrorContainer
        {
            get { return this.moErrorContainer; }
        }

        private string LastErrorMessage(Exception poException)
        {
            if (poException.InnerException == null)
                return poException.Message;
            return this.LastErrorMessage(poException.InnerException);
        }
    }
}

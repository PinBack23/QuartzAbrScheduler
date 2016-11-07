using Abraxas.Scheduler.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abraxas.Scheduler.Core
{
    public class JobExecutionContainer
    {
        private List<JobExecution> moListContainer = new List<JobExecution>();

		#region Singleton

        /// <summary>
        /// Constructor legt Singleton-Instanz an und kann nur einmal aufgerufen werden.
        /// </summary>    
        protected JobExecutionContainer() { }
    
	 	/// <summary>
        /// Instance Property
        /// </summary>
        public static JobExecutionContainer Instance
        {
            get { return NestedSingleton.Instance; }
        }

	 	private class NestedSingleton
        {
            internal static readonly JobExecutionContainer Instance = new JobExecutionContainer ();

	     // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
	     // NICHT ENTFERNEN AUCH WENN FXCOP WAS ANDERES SAGT
            static NestedSingleton() { }
        }
	 	#endregion Singleton

        public void Clear()
        {
            this.moListContainer.Clear();
        }

        public void Add(JobExecution poJobExecution)
        {
            this.moListContainer.Add(poJobExecution);
        }

        public List<JobExecution> ListContainer
        {
            get { return this.moListContainer; }
        }


    }
}

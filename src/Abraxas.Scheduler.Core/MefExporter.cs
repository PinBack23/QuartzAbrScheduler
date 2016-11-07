using Common.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Registration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abraxas.Scheduler.Core
{
    internal class MefExporter
    {
        #region Attributes

        private readonly ILog moLogger = LogManager.GetLogger(Constants.StdLogger);
        private DirectoryCatalog moDirectoryCatalog;
        private CompositionContainer moContainer;

        #endregion

		#region Singleton

        /// <summary>
        /// Constructor legt Singleton-Instanz an und kann nur einmal aufgerufen werden.
        /// </summary>    
        protected MefExporter() 
        {
            var loRegistrationBuilder = new RegistrationBuilder();
            loRegistrationBuilder.ForTypesDerivedFrom<ISchedulerJob>().SetCreationPolicy(CreationPolicy.NonShared).Export<ISchedulerJob>();

            this.moDirectoryCatalog = new DirectoryCatalog(Constants.JobDirectory, loRegistrationBuilder);
            this.moContainer = new CompositionContainer(this.moDirectoryCatalog);
        }
    
	 	/// <summary>
        /// Instance Property
        /// </summary>
        internal static MefExporter Instance
        {
            get { return NestedSingleton.Instance; }
        }

	 	private class NestedSingleton
        {
            internal static readonly MefExporter Instance = new MefExporter ();

	     // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
	     // NICHT ENTFERNEN AUCH WENN FXCOP WAS ANDERES SAGT
            static NestedSingleton() { }
        }
	 	#endregion Singleton

        public void Recompose()
        {
            this.moDirectoryCatalog.Refresh();
        }

        public IEnumerable<ISchedulerJob> GetSchedulerJobs()
        {
            try
            {
                return this.moContainer.GetExportedValues<ISchedulerJob>();
            }
            catch (Exception exp)
            {
                this.moLogger.Error("Fehler beim Initialisieren der Jobs", exp);
                throw;
            }
        }

        public ISchedulerJob GetSchedulerJob(string psType)
        {
            try
            {
                return this.moContainer.GetExportedValues<ISchedulerJob>().FirstOrDefault(item => item.GetType().FullName == psType);
            }
            catch (Exception exp)
            {
                this.moLogger.Error("Fehler beim Initialisieren der Jobs", exp);
                throw;
            }
        }

    }
}

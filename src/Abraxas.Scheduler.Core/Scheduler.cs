using Abraxas.Scheduler.Core.Model;
using Abraxas.Scheduler.Core.Persistence;
using Common.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using Quartz.Impl.Triggers;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Registration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abraxas.Scheduler.Core
{
    public class Scheduler
    {
        #region Attributes

        private readonly ILog moLogger = LogManager.GetLogger(Constants.StdLogger);
        private IScheduler moScheduler;

        #endregion

        #region Singleton

        /// <summary>
        /// Constructor legt Singleton-Instanz an und kann nur einmal aufgerufen werden.
        /// </summary>    
        protected Scheduler()
        {
        }

        /// <summary>
        /// Instance Property
        /// </summary>
        public static Scheduler Instance
        {
            get { return NestedSingleton.Instance; }
        }

        private class NestedSingleton
        {
            internal static readonly Scheduler Instance = new Scheduler();

            // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
            // NICHT ENTFERNEN AUCH WENN FXCOP WAS ANDERES SAGT
            static NestedSingleton() { }
        }

        #endregion Singleton

        public void Start()
        {
            try
            {
                this.Shutdown();

                this.moLogger.InfoFormat("Start Scheduler: {0:G}", DateTime.Now);
                this.moScheduler = StdSchedulerFactory.GetDefaultScheduler();
                this.moScheduler.ListenerManager.AddJobListener(new JobListener());
                this.moScheduler.Start();

                //CronTriggerImpl loTrigger = new CronTriggerImpl("MyCronTrigger");
                //loTrigger.CronExpression = new CronExpression("0/30 * * * * ?");

                //var loTriggerRelationList = TriggerRepository.Instance.Read();

                //foreach (var loSchedulerJob in MefExporter.Instance.GetSchedulerJobs())
                //{
                //    Quartz.Collection.HashSet<ITrigger> loTriggerSet = null;
                //    var loJobDetail = new JobDetailImpl(loSchedulerJob.Name, loSchedulerJob.Group, loSchedulerJob.GetType());
                //    loJobDetail.Description = loSchedulerJob.Description;
                //    foreach (var loTriggerRelation in loTriggerRelationList.Where(item => item.JobName == loJobDetail.Name && item.Group == loJobDetail.Group))
                //    {
                //        if (loTriggerSet == null)
                //            loTriggerSet = new Quartz.Collection.HashSet<ITrigger>();
                //        loTriggerSet.Add(new CronTriggerImpl(loTriggerRelation.TriggerName, loTriggerRelation.Group, loTriggerRelation.CronExpression));
                //    }                    
                //    if (loTriggerSet == null)
                //        this.moScheduler.AddJob(loJobDetail, true, true);
                //    else
                //        this.moScheduler.ScheduleJob(loJobDetail, loTriggerSet, true);
                //}

                foreach (var loJobRelation in JobRepository.Instance.Read())
                    this.AddRelation(loJobRelation);

                this.moLogger.InfoFormat("Add Job done: {0:G}", DateTime.Now);
            }
            catch (Exception exp)
            {
                this.moLogger.Error("Fehler beim Starten des Scheduler", exp);
                throw;
            }
        }

        public void Shutdown()
        {
            if (this.moScheduler != null)
            {
                this.moScheduler.Shutdown();
                this.moScheduler = null;
            }
            JobExecutionContainer.Instance.Clear();
        }

        public bool IsSchedulerStarted()
        {
            if (this.moScheduler == null)
                return false;
            return this.moScheduler.IsStarted;
        }

        public int JobCount()
        {
            int lnCount = 0;
            if (this.moScheduler != null)
            {
                foreach (string lsGroup in this.moScheduler.GetJobGroupNames())
                {
                    var loGroupMatcher = GroupMatcher<JobKey>.GroupContains(lsGroup);
                    lnCount += this.moScheduler.GetJobKeys(loGroupMatcher).Count;
                }
            }
            return lnCount;
        }

        public int TriggerCount()
        {
            int lnCount = 0;
            if (this.moScheduler != null)
            {
                foreach (string lsGroup in this.moScheduler.GetTriggerGroupNames())
                {
                    var loGroupMatcher = GroupMatcher<TriggerKey>.GroupContains(lsGroup);
                    lnCount += this.moScheduler.GetTriggerKeys(loGroupMatcher).Count;
                }
            }
            return lnCount;
        }

        public List<Job> GetJobs()
        {
            List<Job> loList = new List<Job>();
            Job loJob;
            if (this.moScheduler != null)
            {
                foreach (string lsGroup in this.moScheduler.GetJobGroupNames())
                {
                    var loGroupMatcher = GroupMatcher<JobKey>.GroupContains(lsGroup);

                    foreach (var loJobKey in this.moScheduler.GetJobKeys(loGroupMatcher))
                    {
                        var loJobDetail = this.moScheduler.GetJobDetail(loJobKey);
                        loJob = new Job()
                        {
                            Name = loJobKey.Name,
                            Group = lsGroup,
                            Description = loJobDetail.Description,
                            TypeName = loJobDetail.JobType.FullName,
                            AssemblyName = Path.GetFileName(loJobDetail.JobType.Assembly.Location)
                        };
                        this.moScheduler.GetTriggersOfJob(loJobKey).ToList().ForEach(item => loJob.TriggerList.Add(this.CreateTrigger(item)));
                        loList.Add(loJob);

                    }
                }
            }
            return loList;
        }

        public void PauseJob(Job poJob)
        {
            if (this.moScheduler != null)
            {
                this.moScheduler.PauseJob(new JobKey(poJob.Name, poJob.Group));
            }
        }

        public void ToggleJob(Job poJob)
        {
            if (this.moScheduler != null)
            {
                JobKey loJobKey = new JobKey(poJob.Name, poJob.Group);
                bool lbResume = false;

                foreach (var loTrigger in this.moScheduler.GetTriggersOfJob(loJobKey))
                {
                    if (this.moScheduler.GetTriggerState(loTrigger.Key) != TriggerState.Paused)
                    {
                        lbResume = true;
                        break;
                    }
                }

                if (lbResume)
                    this.moScheduler.PauseJob(loJobKey);
                else
                    this.moScheduler.ResumeJob(loJobKey);
            }
        }

        public List<Trigger> GetTrigger()
        {
            List<Trigger> loList = new List<Trigger>();
            if (this.moScheduler != null)
            {
                foreach (string lsGroup in this.moScheduler.GetTriggerGroupNames())
                {
                    var loGroupMatcher = GroupMatcher<TriggerKey>.GroupContains(lsGroup);

                    foreach (var loTriggerKey in this.moScheduler.GetTriggerKeys(loGroupMatcher))
                    {
                        var loTrigger = this.moScheduler.GetTrigger(loTriggerKey);
                        loList.Add(this.CreateTrigger(loTrigger));
                    }
                }
            }
            return loList;
        }

        public void PauseTrigger(Trigger poTrigger)
        {
            if (this.moScheduler != null)
            {
                this.moScheduler.PauseTrigger(new TriggerKey(poTrigger.Name, poTrigger.Group));
            }
        }

        public void ToggleTrigger(Trigger poTrigger)
        {
            if (this.moScheduler != null)
            {
                var loTriggerKey = new TriggerKey(poTrigger.Name, poTrigger.Group);
                switch (this.moScheduler.GetTriggerState(loTriggerKey))
                {
                    case TriggerState.Blocked:
                    case TriggerState.Complete:
                    case TriggerState.Error:
                    case TriggerState.None:
                        break;
                    case TriggerState.Normal:
                        this.moScheduler.PauseTrigger(loTriggerKey);
                        break;
                    case TriggerState.Paused:
                        this.moScheduler.ResumeTrigger(loTriggerKey);
                        break;
                    default:
                        break;
                }
            }
        }

        private Trigger CreateTrigger(ITrigger poTrigger)
        {
            DateTimeOffset? loNextFireTime = poTrigger.GetNextFireTimeUtc();
            DateTimeOffset? loPreviousFireTime = poTrigger.GetPreviousFireTimeUtc();
            CronTriggerImpl loCronTrigger = poTrigger as CronTriggerImpl;

            Trigger loTrigger = new Trigger()
            {
                Name = poTrigger.Key.Name,
                Group = poTrigger.Key.Group,
                TypeName = poTrigger.GetType().FullName,
                State = this.moScheduler.GetTriggerState(poTrigger.Key).ToString()
            };

            if (loNextFireTime.HasValue)
                loTrigger.NextFireTime = loNextFireTime.Value.LocalDateTime;

            if (loPreviousFireTime.HasValue)
                loTrigger.PreviousFireTime = loPreviousFireTime.Value.LocalDateTime;

            if (loCronTrigger != null)
                loTrigger.CronExpression = loCronTrigger.CronExpressionString;

            return loTrigger;
        }

        //public void AddRelation(TriggerRelation poTriggerRelation)
        //{
        //    if (this.moScheduler != null)
        //    {
        //        var loList = TriggerRepository.Instance.Read();
        //        loList.Remove(poTriggerRelation);
        //        loList.Add(poTriggerRelation);
        //        TriggerRepository.Instance.Write(loList);

        //        var loJobKey = this.moScheduler.GetJobKeys(GroupMatcher<JobKey>.GroupContains(poTriggerRelation.Group)).FirstOrDefault(item => item.Name == poTriggerRelation.JobName);
        //        if (loJobKey != null)
        //        {
        //            CronTriggerImpl loTrigger = new CronTriggerImpl(poTriggerRelation.TriggerName, poTriggerRelation.Group, poTriggerRelation.CronExpression);
        //            loTrigger.JobKey = loJobKey;
        //            if (this.moScheduler.GetTriggersOfJob(loJobKey).Any(item => item.Key.Name == loTrigger.Key.Name && item.Key.Group == loTrigger.Key.Group))
        //                this.moScheduler.RescheduleJob(loTrigger.Key, loTrigger);
        //            else
        //                this.moScheduler.ScheduleJob(loTrigger);
        //        }
        //    }
        //}

        public void AddRelation(JobRelation poJobRelation)
        {
            if (this.moScheduler != null)
            {
                var loSchedulerJob = MefExporter.Instance.GetSchedulerJob(poJobRelation.Type);
                if (loSchedulerJob != null)
                {
                    Quartz.Collection.HashSet<ITrigger> loTriggerSet = new Quartz.Collection.HashSet<ITrigger>();
                    loTriggerSet.Add(new CronTriggerImpl("Trg. for {0}".FormatWith(poJobRelation.JobName), poJobRelation.Group, poJobRelation.CronExpression));
                    var loJobDetail = new JobDetailImpl(poJobRelation.JobName, poJobRelation.Group, loSchedulerJob.GetType());
                    loJobDetail.Description = loSchedulerJob.Description;
                    loJobDetail.JobDataMap.Add(Constants.JobConfigEntry, poJobRelation.JsonConfig);
                    this.moScheduler.ScheduleJob(loJobDetail, loTriggerSet, true);
                }
            }
        }

        public void DeleteRelation(JobRelation poJobRelation)
        {
            if (this.moScheduler != null)
            {
                this.moScheduler.DeleteJob(new JobKey(poJobRelation.JobName, poJobRelation.Group));
            }
        }

    }
}

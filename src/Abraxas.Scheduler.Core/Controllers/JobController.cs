using Abraxas.Scheduler.Core.Model;
using Abraxas.Scheduler.Core.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Abraxas.Scheduler.Core.Controllers
{
    public class JobController : ApiController
    {
        public int GetCount()
        {
            return Scheduler.Instance.JobCount();
        }

        public IEnumerable<Job> GetJobs()
        {
            return Scheduler.Instance.GetJobs();
        }

        public void PutPause(Job poJob)
        {
            Scheduler.Instance.PauseJob(poJob);
        }

        public void PutToggle(Job poJob)
        {
            Scheduler.Instance.ToggleJob(poJob);
        }

        public IEnumerable<JobTemplate> GetTemplates()
        {
            foreach (var loScheduleJob in MefExporter.Instance.GetSchedulerJobs())
            {
                yield return new JobTemplate()
                {
                    JobType = loScheduleJob.GetType().FullName,
                    Description = loScheduleJob.Description,
                    DefaultName = loScheduleJob.DefaultName,
                    DefaultGroup = loScheduleJob.DefaultGroup,
                    MandatoryConfigFields = loScheduleJob.MandatoryConfigFields,
                    OptionalConfigFields = loScheduleJob.OptionalConfigFields
                };
            }
        }

        public void PutAddRelation(JobRelation poJobRelation)
        {
            JobRepository.Instance.Write(poJobRelation);
            if (Scheduler.Instance.IsSchedulerStarted())
                Scheduler.Instance.AddRelation(poJobRelation);
        }

        public IEnumerable<JobRelation> GetJobRelations()
        {
            return JobRepository.Instance.Read();
        }

        public void DeleteJobRelation(JobRelation poJobRelation)
        {
            JobRepository.Instance.Delete(poJobRelation);
            if (Scheduler.Instance.IsSchedulerStarted())
                Scheduler.Instance.DeleteRelation(poJobRelation);
        }

        public int GetCountJobExecutionFailed()
        {
            return JobExecutionContainer.Instance.ListContainer.Where(item => !item.Successfully).Count();
        }

        public int GetCountJobExecutionOk()
        {
            return JobExecutionContainer.Instance.ListContainer.Where(item => item.Successfully).Count();
        }

        public IEnumerable<JobExecution> GetJobExecutions(int pnTake = 0)
        {
            if (pnTake == 0)
                return JobExecutionContainer.Instance.ListContainer;
            else
                return JobExecutionContainer.Instance.ListContainer.OrderByDescending(item => item.TimeStamp).Take(pnTake);
        }

    }
}

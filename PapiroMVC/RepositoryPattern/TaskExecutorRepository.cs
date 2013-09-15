using System.Linq;
using PapiroMVC.Models;
using PapiroMVC.DbCodeManagement;
using System;

namespace Services
{

    public class TaskExecutorRepository : GenericRepository<dbEntities, TaskExecutor>, ITaskExecutorRepository
    {
        /// <summary>
        /// Take next code in string numerical order.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public string GetNewCode(TaskExecutor c)
        {
            var codes = (from COD in this.GetAll() select COD.CodTaskExecutor).ToArray().OrderBy(x => x, new SemiNumericComparer());
            var csCode = codes.Count() != 0 ? codes.Last() : "0";
            return AlphaCode.GetNextCode(csCode);
        }

        private void TaskExecutorCostCodeRigen(TaskExecutor c)
        {
            c.TimeStampTable = DateTime.Now;
            int i = 0;
            foreach (var item in c.SetTaskExecutorEstimatedOn.OrderBy(x=>x.CodTaskExecutorOn))
            {

                item.CodTaskExecutorOn = c.CodTaskExecutor + item.TypeOfEstimatedOn.ToString()+ (i++).ToString();
                foreach (var step in item.steps)
                {
                    item.TimeStampTable = DateTime.Now;
                    step.CodTaskEstimatedOn = item.CodTaskExecutorOn;
                }
            }
        }

        public override void Add(TaskExecutor entity)
        {
            TaskExecutorCostCodeRigen(entity);

            //cehck if name is just inserted
            var taskExecutor = (from ART in this.GetAll() where ART.TaskExecutorName == entity.TaskExecutorName select ART);
            if (taskExecutor.Count() > 0)
            {
                //this.Edit(entity);
            }
            else
                base.Add(entity);
        }

        public override IQueryable<TaskExecutor> GetAll()
        {
            return Context.taskexecutors.Include("SetTaskExecutorEstimatedOn").Include("SetTaskExecutorEstimatedOn.steps");
        }

        public override void Edit(TaskExecutor entity)
        {
            TaskExecutorCostCodeRigen(entity);

            entity.TimeStampTable = DateTime.Now;
            foreach (var item in entity.SetTaskExecutorEstimatedOn)
            {
                item.TimeStampTable = DateTime.Now;
                //Context.Entry(item).State = System.Data.EntityState.Modified;
                foreach (var item2 in item.steps)
                {
                    item2.TimeStampTable = DateTime.Now;
                    Console.WriteLine(Context.Entry(item2).State);
                    switch (Context.Entry(item2).State)
                    {
                        case System.Data.EntityState.Added:
                            break;
                        case System.Data.EntityState.Deleted:
                            break;
                        case System.Data.EntityState.Detached:
                            break;
                        case System.Data.EntityState.Modified:
                            break;
                        case System.Data.EntityState.Unchanged:
                            Context.Entry(item2).State = System.Data.EntityState.Modified;                    
                            break;
                        default:
                            break;
                    }
                }
            }
            base.Edit(entity);
        }

        public TaskEstimatedOn GetSingleEstimatedOn(string cod)
        {
            return (this.Context.taskexecutorestimatedon.Include("steps").Include("taskexecutors").First(x => x.CodTaskExecutorOn == cod));           
        }

        public Step GetSingleStep(int cod)
        {
            return (this.Context.steps.Include("taskexecutorestimatedon").Include("taskexecutorestimatedon.taskexecutors").First(x => x.IdStep == cod));
        }

        public TaskExecutor GetSingle(string codTaskExecutor)
        {
            var query = this.GetAll().FirstOrDefault(x => x.CodTaskExecutor == codTaskExecutor);
            return query;
        }
    }
}
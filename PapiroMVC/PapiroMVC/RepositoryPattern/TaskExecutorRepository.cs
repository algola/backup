using System.Linq;
using PapiroMVC.Models;
using PapiroMVC.DbCodeManagement;
using System;

namespace Services
{

    public class TaskExecutorRepository : GenericRepository<dbEntities, TaskExecutor>, ITaskExecutorRepository
    {
        public void AddEstimatedOn(TaskEstimatedOn item)
        {
            if (Context.Entry(item).State != System.Data.Entity.EntityState.Added)
            {
                var tskEst = item;
                var fromBD2 = Context.taskexecutorestimatedon.Single(p => p.CodTaskEstimatedOn == tskEst.CodTaskEstimatedOn);
                Context.Entry(fromBD2).CurrentValues.SetValues(tskEst);
            }
        
        }

        /// <summary>
        /// Take next code in string numerical order.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public string GetNewCode(TaskExecutor c)
        {
            var csCode = (from COD in this.GetAll() select COD.CodTaskExecutor).Max();
            return AlphaCode.GetNextCode(csCode ?? "0").PadLeft(6, '0');
        }

        private void TaskExecutorCostCodeRigen(TaskExecutor c)
        {
            c.TimeStampTable = DateTime.Now;
            int i = 0;
            foreach (var item in c.SetTaskExecutorEstimatedOn.OrderBy(x=>x.CodTaskEstimatedOn))
            {

                item.CodTaskEstimatedOn = c.CodTaskExecutor + item.TypeOfEstimatedOn.ToString()+ (i++).ToString();
                foreach (var step in item.steps)
                {
                    item.TimeStampTable = DateTime.Now;
                    step.CodTaskEstimatedOn = item.CodTaskEstimatedOn;
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
                //Context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                foreach (var item2 in item.steps)
                {
                    item2.TimeStampTable = DateTime.Now;
                    Console.WriteLine(Context.Entry(item2).State);
                    switch (Context.Entry(item2).State)
                    {
                        case System.Data.Entity.EntityState.Added:
                            break;
                        case System.Data.Entity.EntityState.Deleted:
                            break;
                        case System.Data.Entity.EntityState.Detached:
                            break;
                        case System.Data.Entity.EntityState.Modified:
                            break;
                        case System.Data.Entity.EntityState.Unchanged:
                            Context.Entry(item2).State = System.Data.Entity.EntityState.Modified;                    
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
            return (this.Context.taskexecutorestimatedon.Include("steps").Include("taskexecutors").First(x => x.CodTaskEstimatedOn == cod));           
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
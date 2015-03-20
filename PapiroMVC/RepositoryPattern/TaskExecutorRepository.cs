using System.Linq;
using PapiroMVC.Models;
using PapiroMVC.DbCodeManagement;
using System;
using System.Collections.Generic;

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
                Context.Entry(fromBD2).State = System.Data.Entity.EntityState.Modified;
            }

        }


        public override void Save()
        {
            List<Object> modOrAdded = Context.ChangeTracker.Entries()
               .Where(x => x.State == System.Data.Entity.EntityState.Modified
               || x.State == System.Data.Entity.EntityState.Added)
               .Select(x => x.Entity).ToList();

            Console.Write(modOrAdded);

                base.Save();
        }

        /// <summary>
        /// Confirm cylinder modify
        /// </summary>
        /// <param name="entity"></param>
        public void EditSingleCylinder(TaskExecutorCylinder entity)
        {
            Context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
        }

        /// <summary>
        /// Delete cyl
        /// </summary>
        /// <param name="entity"></param>
        public void DeleteSingleCylinder(TaskExecutorCylinder entity)
        {
            Context.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
        }


        /// <summary>
        /// Confirm cylinder modify
        /// </summary>
        /// <param name="entity"></param>
        public void AddSingleCylinder(TaskExecutorCylinder entity)
        {
            Context.Entry(entity).State = System.Data.Entity.EntityState.Added;
        }

        public void EditSingleStep(Step entity)
        {
            Context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
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

            foreach (var item in c.SetTaskExecutorEstimatedOn.OrderBy(x => x.CodTaskEstimatedOn, new EmptyStringsAreLast()))
            {
                item.CodTaskExecutor = c.CodTaskExecutor;
                item.CodTaskEstimatedOn = c.CodTaskExecutor + item.TypeOfEstimatedOn.ToString() + (i++).ToString();
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

            var tsks = Context.taskexecutors.Where(x => x.CodTypeOfTask != "STAMPAETICHROTOLO" &&
                x.CodTypeOfTask != "STAMPAOFFeDIGITALE" &&
                x.CodTypeOfTask != "STAMPAMORBIDO" &&
                x.CodTypeOfTask != "STAMPARIGIDO").ToList();

            foreach (var tsk in tsks)
            {
                switch (tsk.TypeOfExecutor)
                {
                    case TaskExecutor.ExecutorType.LithoSheet:
                    case TaskExecutor.ExecutorType.DigitalSheet:
                        tsk.CodTypeOfTask = "STAMPAOFFeDIGITALE";
                        break;
                    case TaskExecutor.ExecutorType.Flexo:
                        tsk.CodTypeOfTask = "STAMPAETICHROTOLO";
                        break;
                    case TaskExecutor.ExecutorType.LithoRoll:
                    case TaskExecutor.ExecutorType.DigitalRoll:
                        break;
                    case TaskExecutor.ExecutorType.PlotterSheet:
                        tsk.CodTypeOfTask = "STAMPARIGIDO";
                        break;
                    case TaskExecutor.ExecutorType.PlotterRoll:
                        tsk.CodTypeOfTask = "STAMPAMORBIDO";
                        break;
                    case TaskExecutor.ExecutorType.Binding:
                    case TaskExecutor.ExecutorType.FlatRoll:
                    case TaskExecutor.ExecutorType.ControlTableRoll:
                    case TaskExecutor.ExecutorType.PrePostPress:
                        Console.WriteLine(tsk.CodTypeOfTask);
                        break;
                    default:
                        break;

                }
                Edit(tsk);
                Save();
            }



            return Context.taskexecutors.Include("SetTaskExecutorEstimatedOn").Include("SetTaskExecutorEstimatedOn.steps").Include("TaskExecutorCylinders");
        }

        public override void Edit(TaskExecutor entity)
        {
            TaskExecutorCostCodeRigen(entity);

            entity.TimeStampTable = DateTime.Now;
            foreach (var item in entity.SetTaskExecutorEstimatedOn)
            {
                item.TimeStampTable = DateTime.Now;
                var fromBD2 = Context.taskexecutorestimatedon.SingleOrDefault(p => p.CodTaskEstimatedOn == item.CodTaskEstimatedOn);
                if (fromBD2 != null)
                {
                    Context.Entry(fromBD2).CurrentValues.SetValues(item);
                    Context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    Context.Entry(item).State = System.Data.Entity.EntityState.Added;
                }

                foreach (var item2 in item.steps)
                {
                    item2.TimeStampTable = DateTime.Now;
                    var fromBD3 = Context.steps.SingleOrDefault(p => p.IdStep == item2.IdStep);
                    if (fromBD3 != null)
                    {
                        Context.Entry(fromBD3).CurrentValues.SetValues(item2);
                    }
                    else
                    {
                        Context.Entry(item2).State = System.Data.Entity.EntityState.Added;
                    }
                }

            }

            /*
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
                        }*/


            entity.TimeStampTable = DateTime.Now;
            var fromBD = Context.taskexecutors.SingleOrDefault(p => p.CodTaskExecutor == entity.CodTaskExecutor);
            if (fromBD != null)
            {
                Context.Entry(fromBD).CurrentValues.SetValues(entity);
                Context.Entry(fromBD).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public TaskExecutorCylinder GetSingleTaskExecutorCylindern(string cod)
        {
            return this.Context.TaskExecutorCylinders.FirstOrDefault(x => x.CodTaskExecutorCylinder == cod);
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
            if (query != null)
            {
                query.SetTaskExecutorEstimatedOn = this.Context.taskexecutorestimatedon.Include("steps").Where(x => x.CodTaskExecutor == codTaskExecutor).ToList();


                // query.TaskExecutorCylinders = this.Context
            }
            return query;
        }
    }
}
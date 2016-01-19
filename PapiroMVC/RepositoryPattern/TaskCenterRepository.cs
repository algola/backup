using System.Linq;
using PapiroMVC.Models;
using PapiroMVC.DbCodeManagement;
using System;

namespace Services
{

    public class TaskCenterRepository : GenericRepository<dbEntities, TaskCenter>, ITaskCenterRepository
    {
        /// <summary>
        /// Take next code in string numerical order.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public string GetNewCode(TaskCenter c)
        {
            var csCode = (from COD in this.GetAll() select COD.CodTaskCenter).Max();
            return AlphaCode.GetNextCode(csCode ?? "0").PadLeft(6, '0');
        }

        public void AddNewDocumentTaskCenter(DocumentTaskCenter entity)
        {

            entity.CodDocumentTaskCenter = entity.CodDocument;
            entity.TimeStampTable = DateTime.Now;


            var lastIndex = this.GetDocumentsTaskCenter(entity.CodTaskCenter).Max(x => x.IndexOf);
            if (lastIndex == null)
            {
                entity.IndexOf = 0;
            }
            else
            {
                entity.IndexOf = lastIndex + 1;
            }


            var fromBD = Context.DocumentTaskCenters.SingleOrDefault(p => p.CodDocumentTaskCenter == entity.CodDocumentTaskCenter);
            if (fromBD != null)
            {
                Context.Entry(fromBD).CurrentValues.SetValues(entity);
                Context.Entry(fromBD).State = System.Data.Entity.EntityState.Modified;

            }
            else
            {
                Context.Entry(entity).State = System.Data.Entity.EntityState.Added;
            }

        }

        public void EditDocumentTaskCenter(DocumentTaskCenter entity)
        {

            entity.TimeStampTable = DateTime.Now;

            var fromBD = Context.DocumentTaskCenters.SingleOrDefault(p => p.CodDocumentTaskCenter == entity.CodDocumentTaskCenter);
            if (fromBD != null)
            {
                Context.Entry(fromBD).CurrentValues.SetValues(entity);
                Context.Entry(fromBD).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public IQueryable<DocumentTaskCenter> GetDocumentsTaskCenter(string codTaskCenter)
        {
            if (codTaskCenter != String.Empty)
            {
                return Context.DocumentTaskCenters.Where(x => x.CodTaskCenter == codTaskCenter);                
            }
            else
            {
                return Context.DocumentTaskCenters;
            }
        }

        public DocumentTaskCenter GetDocumentTaskCenter(string codDocumentTaskCenter)
        {
            return Context.DocumentTaskCenters.Where(x => x.CodDocumentTaskCenter == codDocumentTaskCenter).FirstOrDefault();
        }

        public override IQueryable<TaskCenter> GetAll()
        {

            var tcs = Context.TaskCenters.Include("State").ToList();

            foreach (var c in tcs)
            {
                c.TaskCenters = tcs.Select(x => x.CodTaskCenter).ToArray();
            }

            return Context.TaskCenters;

        }

        public override void Edit(TaskCenter entity)
        {
            base.Edit(entity);
        }

        public TaskCenter GetSingle(string codTaskCenter)
        {
            var query = Context.TaskCenters.FirstOrDefault(x => x.CodTaskCenter == codTaskCenter);
            query.TaskCenters = GetAll().Select(x => x.CodTaskCenter).ToArray();

            return query;
        }

        public override void Add(TaskCenter entity)
        {
            entity.TimeStampTable = DateTime.Now;
            base.Add(entity);
        }

        public void DeleteDocumentTaskCenter(DocumentTaskCenter entity)
        {
            this.Context.Set<DocumentTaskCenter>().Remove(entity);
        }
    }
}
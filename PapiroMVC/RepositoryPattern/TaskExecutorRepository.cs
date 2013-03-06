using System.Linq;
using PapiroMVC.Models;
using PapiroMVC.DbCodeManagement;

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

            var csCode = codes.Last();

            if (csCode == null)
                csCode = "0";
            return AlphaCode.GetNextCode(csCode);

        }

        public override IQueryable<TaskExecutor> GetAll()
        {
            return Context.taskexecutors;
        }

        public override void Edit(TaskExecutor entity)
        {
            foreach (var item in entity.SetTaskExecutorEstimatedOn)
            {
                Context.Entry(item).State = System.Data.EntityState.Modified;
            }
            base.Edit(entity);
        }

        public TaskExecutor GetSingle(string codTaskExecutor)
        {
            var query = Context.taskexecutors.FirstOrDefault(x => x.CodTaskExecutor == codTaskExecutor);
            return query;
        }
    }
}
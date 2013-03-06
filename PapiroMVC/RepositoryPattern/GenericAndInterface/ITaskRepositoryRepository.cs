using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

namespace Services
{
    public interface ITaskExecutorRepository : IGenericRepository<TaskExecutor>
    {
        string GetNewCode(TaskExecutor c);
        TaskExecutor GetSingle(string cod);
    }

}

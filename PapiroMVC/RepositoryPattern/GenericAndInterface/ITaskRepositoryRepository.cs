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
        new TaskExecutor GetSingle(string cod);
        TaskEstimatedOn GetSingleEstimatedOn(string cod);
        Step GetSingleStep(int cod);
    }

}

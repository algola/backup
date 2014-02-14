using PapiroMVC.Models.Resources.TaskExecutor;
using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class TypeOfTask_MetaData
    {
        //[DisplayNameLocalized(typeof(ResTaskExecutor), "TimeStampTable")]
        //[Tooltip(typeof(ResTaskExecutor), "TimeStampTableToolTip")]
        //public string CodTypeOfTask { get; set; }
        //public string TaskName { get; set; }
        public string CodCategoryOfTask { get; set; }

        //public virtual ICollection<TaskExecutorTypeOfTask> TaskExecutorTypeOfTasks { get; set; }
    }
}
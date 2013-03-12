using PapiroMVC.Models.Resources.TaskExecutor;
using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public abstract partial class TaskEstimatedOn_MetaData
    {
        [DisplayNameLocalized(typeof(ResTaskEstimatedOn), "TimeStampTable")]
        [Tooltip(typeof(ResTaskEstimatedOn), "TimeStampTableToolTip")]
        public System.DateTime TimeStampTable { get; set; }
        [DisplayNameLocalized(typeof(ResTaskEstimatedOn), "CodTaskExecutor")]
        [Tooltip(typeof(ResTaskEstimatedOn), "CodTaskExecutorToolTip")]
        public string CodTaskExecutor { get; set; }
        [DisplayNameLocalized(typeof(ResTaskEstimatedOn), "CodTaskExecutorOn")]
        [Tooltip(typeof(ResTaskEstimatedOn), "CodTaskExecutorOnToolTip")]
        public string CodTaskExecutorOn { get; set; }
        [DisplayNameLocalized(typeof(ResTaskEstimatedOn), "CostForfait")]
        [Tooltip(typeof(ResTaskEstimatedOn), "CostForfaitToolTip")]
        public Nullable<double> CostForfait { get; set; }
        [DisplayNameLocalized(typeof(ResTaskEstimatedOn), "CostForfaitForSheet")]
        [Tooltip(typeof(ResTaskEstimatedOn), "CostForfaitForSheetToolTip")]
        public Nullable<double> CostForfaitForSheet { get; set; }
        [DisplayNameLocalized(typeof(ResTaskEstimatedOn), "taskexecutors")]
        [Tooltip(typeof(ResTaskEstimatedOn), "taskexecutorsToolTip")]
        public virtual TaskExecutor taskexecutors { get; set; }
        [DisplayNameLocalized(typeof(ResTaskEstimatedOn), "steps")]
        [Tooltip(typeof(ResTaskEstimatedOn), "stepsToolTip")]
        public virtual ICollection<Step> steps { get; set; }
    }
}
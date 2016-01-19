using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using PapiroMVC.Models.Resources.TaskExecutor;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{

    public partial class TaskCenter_MetaData
    {
        [Required(ErrorMessageResourceType = typeof(ResTaskExecutor), ErrorMessageResourceName = "RequiredField"),
        DisplayNameLocalized(typeof(ResTaskExecutor), "TaskCenterName"), Tooltip(typeof(ResTaskExecutor), "TaskCenterNameToolTip")]
        public string TaskCenterName { get; set; }

        [Required(ErrorMessageResourceType = typeof(ResTaskExecutor), ErrorMessageResourceName = "RequiredField"),
        DisplayNameLocalized(typeof(ResTaskExecutor), "IndexOf"), Tooltip(typeof(ResTaskExecutor), "IndexOfToolTip")]
        public Nullable<int> IndexOf { get; set; }

        [DisplayNameLocalized(typeof(ResTaskExecutor), "AlarmStartAfterDays"), Tooltip(typeof(ResTaskExecutor), "AlarmStartAfterDaysToolTip")]
        public Nullable<int> AlarmStartAfterDays { get; set; }

        [DisplayNameLocalized(typeof(ResTaskExecutor), "AlarmFinishAfterDays"), Tooltip(typeof(ResTaskExecutor), "AlarmFinishAfterDaysToolTip")]
        public Nullable<int> AlarmFinishAfterDays { get; set; }

        [DisplayNameLocalized(typeof(ResTaskExecutor), "StateName"), Tooltip(typeof(ResTaskExecutor), "StateNameToolTip")]
        public string StateName
        {
            get;
            set;
        }
    
    }
}
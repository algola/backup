using PapiroMVC.Models.Resources.TaskExecutor;
using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public abstract partial class TaskExecutor_MetaData
    {

        [DisplayNameLocalized(typeof(ResTaskExecutor), "TimeStampTable")]
        [Tooltip(typeof(ResTaskExecutor), "TimeStampTableToolTip")]
        public System.DateTime TimeStampTable { get; set; }

        [DisplayNameLocalized(typeof(ResTaskExecutor), "CodTaskExecutor")]
        [Tooltip(typeof(ResTaskExecutor), "CodTaskExecutorToolTip")]
        public string CodTaskExecutor { get; set; }

        [DisplayNameLocalized(typeof(ResTaskExecutor), "TaskExecutorName")]
        [Tooltip(typeof(ResTaskExecutor), "TaskExecutorNameToolTip")]
        [Required(ErrorMessageResourceType = typeof(ResTaskExecutor), ErrorMessageResourceName = "RequiredField")]
        public string TaskExecutorName { get; set; }

        [DisplayNameLocalized(typeof(ResTaskExecutor), "Version")]
        [Tooltip(typeof(ResTaskExecutor), "VersionToolTip")]
        public string Version { get; set; }

        [DisplayNameLocalized(typeof(ResTaskExecutor), "Dismissed")]
        [Tooltip(typeof(ResTaskExecutor), "DismissedToolTip")]
        public Nullable<bool> Dismissed { get; set; }

        [DisplayNameLocalized(typeof(ResTaskExecutor), "FormatMin")]
        [Tooltip(typeof(ResTaskExecutor), "FormatMinToolTip")]
        [RegularExpressionLocalizedAttribute(typeof(ResTaskExecutor),"FormatValidation","FormatValidationError")]
        [Required(ErrorMessageResourceType = typeof(ResTaskExecutor), ErrorMessageResourceName = "RequiredField")]
        public string FormatMin { get; set; }

        [DisplayNameLocalized(typeof(ResTaskExecutor), "FormatMax")]
        [Tooltip(typeof(ResTaskExecutor), "FormatMaxToolTip")]
        [RegularExpressionLocalizedAttribute(typeof(ResTaskExecutor), "FormatValidation", "FormatValidationError")]
        [Required(ErrorMessageResourceType = typeof(ResTaskExecutor), ErrorMessageResourceName = "RequiredField")]
        public string FormatMax { get; set; }

        [DisplayNameLocalized(typeof(ResTaskExecutor), "WeightMin")]
        [Tooltip(typeof(ResTaskExecutor), "WeightMinToolTip")]
        public Nullable<long> WeightMin { get; set; }

        [DisplayNameLocalized(typeof(ResTaskExecutor), "WeightMax")]
        [Tooltip(typeof(ResTaskExecutor), "WeightMaxToolTip")]
        public Nullable<long> WeightMax { get; set; }

        [DisplayNameLocalized(typeof(ResTaskExecutor), "Pinza")]
        [Tooltip(typeof(ResTaskExecutor), "PinzaToolTip")]
        public Nullable<double> Pinza { get; set; }

        [DisplayNameLocalized(typeof(ResTaskExecutor), "ControPinza")]
        [Tooltip(typeof(ResTaskExecutor), "ControPinzaToolTip")]
        public Nullable<double> ControPinza { get; set; }

        [DisplayNameLocalized(typeof(ResTaskExecutor), "Laterale")]
        [Tooltip(typeof(ResTaskExecutor), "LateraleToolTip")]
        public Nullable<double> Laterale { get; set; }

        public Nullable<bool> HasImplant { get; set; }

       
        //[DisplayNameLocalized(typeof(ResTaskExecutor), "IsEstimatedOnTime")]
        //[Tooltip(typeof(ResTaskExecutor), "IsEstimatedOnTimeToolTip")]
        //public Nullable<bool> IsEstimatedOnTime { get; set; }

        //[DisplayNameLocalized(typeof(ResTaskExecutor), "IsEstimatedOnRun")]
        //[Tooltip(typeof(ResTaskExecutor), "IsEstimatedOnRunToolTip")]
        //public Nullable<bool> IsEstimatedOnRun { get; set; }

        //[DisplayNameLocalized(typeof(ResTaskExecutor), "IsEstimatedOnMq")]
        //[Tooltip(typeof(ResTaskExecutor), "IsEstimatedOnMqToolTip")]
        //public Nullable<bool> IsEstimatedOnMq { get; set; }

        [DisplayNameLocalized(typeof(ResTaskExecutor), "SetTaskExecutorEstimatedOn")]
        [Tooltip(typeof(ResTaskExecutor), "SetTaskExecutorEstimatedOnToolTip")]
        public virtual ICollection<TaskEstimatedOn> SetTaskExecutorEstimatedOn { get; set; }
    }
}
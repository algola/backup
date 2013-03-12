using PapiroMVC.Models.Resources.TaskExecutor;
using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{

    public abstract partial class TaskEstimatedOnUnit_MetaData : TaskEstimatedOn_MetaData
    {
        [DisplayNameLocalized(typeof(ResTaskEstimatedOnUnit), "CostPerUnit")]
        [Tooltip(typeof(ResTaskEstimatedOnUnit), "CostPerUnitToolTip")]
        public Nullable<double> CostPerUnit { get; set; }
        [DisplayNameLocalized(typeof(ResTaskEstimatedOnUnit), "UseDifferentCostPerUnit")]
        [Tooltip(typeof(ResTaskEstimatedOnUnit), "UseDifferentCostPerUnitToolTip")]
        public Nullable<bool> UseDifferentCostPerUnit { get; set; }
        [DisplayNameLocalized(typeof(ResTaskEstimatedOnUnit), "StartingCost1")]
        [Tooltip(typeof(ResTaskEstimatedOnUnit), "StartingCost1ToolTip")]
        public Nullable<double> StartingCost1 { get; set; }
        [DisplayNameLocalized(typeof(ResTaskEstimatedOnUnit), "StartingCost2")]
        [Tooltip(typeof(ResTaskEstimatedOnUnit), "StartingCost2ToolTip")]
        public Nullable<double> StartingCost2 { get; set; }
        [DisplayNameLocalized(typeof(ResTaskEstimatedOnUnit), "UseDifferentDeficitOnCostForWeightStep")]
        [Tooltip(typeof(ResTaskEstimatedOnUnit), "UseDifferentDeficitOnCostForWeightStepToolTip")]
        public Nullable<bool> UseDifferentDeficitOnCostForWeightStep { get; set; }
    }
}

using PapiroMVC.Models.Resources.TaskExecutor;
using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class TaskEstimatedOnTime_MetaData : TaskEstimatedOn_MetaData
    {
        [DisplayNameLocalized(typeof(ResTaskEstimatedOnTime), "AvarageRunPerHour")]
        [Tooltip(typeof(ResTaskEstimatedOnTime), "AvarageRunPerHourToolTip")]
        public Nullable<long> AvarageRunPerHour { get; set; }
        [DisplayNameLocalized(typeof(ResTaskEstimatedOnTime), "UseDifferentRunPerHour")]
        [Tooltip(typeof(ResTaskEstimatedOnTime), "UseDifferentRunPerHourToolTip")]
        public Nullable<bool> UseDifferentRunPerHour { get; set; }
        [DisplayNameLocalized(typeof(ResTaskEstimatedOnTime), "StartingTime1")]
        [Tooltip(typeof(ResTaskEstimatedOnTime), "StartingTime1ToolTip")]
        public Nullable<System.TimeSpan> StartingTime1 { get; set; }
        [DisplayNameLocalized(typeof(ResTaskEstimatedOnTime), "StartingTime2")]
        [Tooltip(typeof(ResTaskEstimatedOnTime), "StartingTime2ToolTip")]
        public Nullable<System.TimeSpan> StartingTime2 { get; set; }
        [DisplayNameLocalized(typeof(ResTaskEstimatedOnTime), "TimeForfait")]
        [Tooltip(typeof(ResTaskEstimatedOnTime), "TimeForfaitToolTip")]
        public Nullable<System.TimeSpan> TimeForfait { get; set; }
        [DisplayNameLocalized(typeof(ResTaskEstimatedOnTime), "CostPerHourRunning")]
        [Tooltip(typeof(ResTaskEstimatedOnTime), "CostPerHourRunningToolTip")]
        public Nullable<double> CostPerHourRunning { get; set; }
        [DisplayNameLocalized(typeof(ResTaskEstimatedOnTime), "CostPerHourStarting")]
        [Tooltip(typeof(ResTaskEstimatedOnTime), "CostPerHourStartingToolTip")]
        public Nullable<double> CostPerHourStarting { get; set; }
        [DisplayNameLocalized(typeof(ResTaskEstimatedOnTime), "UseDifferentDeficitForWeightStep")]
        [Tooltip(typeof(ResTaskEstimatedOnTime), "UseDifferentDeficitForWeightStepToolTip")]
        public Nullable<bool> UseDifferentDeficitForWeightStep { get; set; }
    }
}
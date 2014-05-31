using PapiroMVC.Models.Resources.TaskExecutor;
using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class ControlTableRollEstimatedOnTime_MetaData : TaskEstimatedOnTime_MetaData
    {
        [DisplayNameLocalized(typeof(ResControlTableRollEstimatedOnTime), "AvarageRunPerHour")]
        [Tooltip(typeof(ResControlTableRollEstimatedOnTime), "AvarageRunPerHourToolTip")]
        public Nullable<long> AvarageRunPerHour { get; set; }
        [DisplayNameLocalized(typeof(ResControlTableRollEstimatedOnTime), "UseDifferentRunPerHour")]
        [Tooltip(typeof(ResControlTableRollEstimatedOnTime), "UseDifferentRunPerHourToolTip")]
        public Nullable<bool> UseDifferentRunPerHour { get; set; }
        [DisplayNameLocalized(typeof(ResControlTableRollEstimatedOnTime), "StartingTime1")]
        [Tooltip(typeof(ResControlTableRollEstimatedOnTime), "StartingTime1ToolTip")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = @"{0:hh\:mm}")]
        public Nullable<System.TimeSpan> StartingTime1 { get; set; }
        [DisplayNameLocalized(typeof(ResControlTableRollEstimatedOnTime), "StartingTime2")]
        [Tooltip(typeof(ResControlTableRollEstimatedOnTime), "StartingTime2ToolTip")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = @"{0:hh\:mm}")]
        public Nullable<System.TimeSpan> StartingTime2 { get; set; }
        [DisplayNameLocalized(typeof(ResControlTableRollEstimatedOnTime), "TimeForfait")]
        [Tooltip(typeof(ResControlTableRollEstimatedOnTime), "TimeForfaitToolTip")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = @"{0:hh\:mm}")]
        public Nullable<System.TimeSpan> TimeForfait { get; set; }

        //[DisplayNameLocalized(typeof(ResControlTableRollEstimatedOnTime), "CostPerHourRunning")]
        //[Tooltip(typeof(ResControlTableRollEstimatedOnTime), "CostPerHourRunningToolTip")]
        //[RegularExpressionLocalized(typeof(ResControlTableRollEstimatedOnTime), "CurrencyValidation", "CurrencyValidationError")]
        //public string CostPerHourRunning { get; set; }
        //[DisplayNameLocalized(typeof(ResControlTableRollEstimatedOnTime), "CostPerHourStarting")]
        //[Tooltip(typeof(ResControlTableRollEstimatedOnTime), "CostPerHourStartingToolTip")]
        //[RegularExpressionLocalized(typeof(ResControlTableRollEstimatedOnTime), "CurrencyValidation", "CurrencyValidationError")]
        public string CostPerHourStarting { get; set; }
        [DisplayNameLocalized(typeof(ResControlTableRollEstimatedOnTime), "UseDifferentDeficitForWeightStep")]
        [Tooltip(typeof(ResControlTableRollEstimatedOnTime), "UseDifferentDeficitForWeightStepToolTip")]
        public Nullable<bool> UseDifferentDeficitForWeightStep { get; set; }
        [DisplayNameLocalized(typeof(ResControlTableRollEstimatedOnTime), "StartingTimePerColor")]
        [Tooltip(typeof(ResControlTableRollEstimatedOnTime), "StartingTimePerColorToolTip")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = @"{0:hh\:mm}")]
        public Nullable<System.TimeSpan> StartingTimePerColor { get; set; }
    }
}
using PapiroMVC.Models.Resources.TaskExecutor;
using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class RollEstimatedOnTime_MetaData : TaskEstimatedOnTime_MetaData
    {
        [DisplayNameLocalized(typeof(ResRollEstimatedOnTime), "AvarageRunPerHour")]
        [Tooltip(typeof(ResRollEstimatedOnTime), "AvarageRunPerHourToolTip")]
        public Nullable<long> AvarageRunPerHour { get; set; }
        [DisplayNameLocalized(typeof(ResRollEstimatedOnTime), "UseDifferentRunPerHour")]
        [Tooltip(typeof(ResRollEstimatedOnTime), "UseDifferentRunPerHourToolTip")]
        public Nullable<bool> UseDifferentRunPerHour { get; set; }
        [DisplayNameLocalized(typeof(ResRollEstimatedOnTime), "StartingTime1")]
        [Tooltip(typeof(ResRollEstimatedOnTime), "StartingTime1ToolTip")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = @"{0:hh\:mm}")]
        public Nullable<System.TimeSpan> StartingTime1 { get; set; }
        [DisplayNameLocalized(typeof(ResRollEstimatedOnTime), "StartingTime2")]
        [Tooltip(typeof(ResRollEstimatedOnTime), "StartingTime2ToolTip")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = @"{0:hh\:mm}")]
        public Nullable<System.TimeSpan> StartingTime2 { get; set; }
        [DisplayNameLocalized(typeof(ResRollEstimatedOnTime), "TimeForfait")]
        [Tooltip(typeof(ResRollEstimatedOnTime), "TimeForfaitToolTip")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = @"{0:hh\:mm}")]
        public Nullable<System.TimeSpan> TimeForfait { get; set; }

        //[DisplayNameLocalized(typeof(ResRollEstimatedOnTime), "CostPerHourRunning")]
        //[Tooltip(typeof(ResRollEstimatedOnTime), "CostPerHourRunningToolTip")]
        //[RegularExpressionLocalized(typeof(ResRollEstimatedOnTime), "CurrencyValidation", "CurrencyValidationError")]
        //public string CostPerHourRunning { get; set; }
        //[DisplayNameLocalized(typeof(ResRollEstimatedOnTime), "CostPerHourStarting")]
        //[Tooltip(typeof(ResRollEstimatedOnTime), "CostPerHourStartingToolTip")]
        //[RegularExpressionLocalized(typeof(ResRollEstimatedOnTime), "CurrencyValidation", "CurrencyValidationError")]
        public string CostPerHourStarting { get; set; }
        [DisplayNameLocalized(typeof(ResRollEstimatedOnTime), "UseDifferentDeficitForWeightStep")]
        [Tooltip(typeof(ResRollEstimatedOnTime), "UseDifferentDeficitForWeightStepToolTip")]
        public Nullable<bool> UseDifferentDeficitForWeightStep { get; set; }
        [DisplayNameLocalized(typeof(ResRollEstimatedOnTime), "StartingTimePerColor")]
        [Tooltip(typeof(ResRollEstimatedOnTime), "StartingTimePerColorToolTip")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = @"{0:hh\:mm}")]
        public Nullable<System.TimeSpan> StartingTimePerColor { get; set; }
    }
}
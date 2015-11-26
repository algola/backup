using PapiroMVC.Models.Resources.TaskExecutor;
using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class FlatRollEstimatedOnTime_MetaData : TaskEstimatedOnTime_MetaData
    {
        [DisplayNameLocalized(typeof(ResRollEstimatedOnTime), "AvarageRunPerHourFlat")]
        [Tooltip(typeof(ResRollEstimatedOnTime), "AvarageRunPerHourFlatToolTip")]
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


        
        [DisplayNameLocalized(typeof(ResRollEstimatedOnTime), "UseDifferentDeficitForWeightStep")]
        [Tooltip(typeof(ResRollEstimatedOnTime), "UseDifferentDeficitForWeightStepToolTip")]
        public Nullable<bool> UseDifferentDeficitForWeightStep { get; set; }
        [DisplayNameLocalized(typeof(ResRollEstimatedOnTime), "StartingTimePerColor")]
        [Tooltip(typeof(ResRollEstimatedOnTime), "StartingTimePerColorToolTip")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = @"{0:hh\:mm}")]
        public Nullable<System.TimeSpan> StartingTimePerColor { get; set; }


        [DisplayNameLocalized(typeof(ResRollEstimatedOnTime), "StartingTimeRetro")]
        [Tooltip(typeof(ResRollEstimatedOnTime), "StartingTimeRetroToolTip")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = @"{0:hh\:mm}")]
        public Nullable<System.TimeSpan> StartingTimeRetro { get; set; }


        [DisplayNameLocalized(typeof(ResRollEstimatedOnTime), "StartingTimeSerigraphy")]
        [Tooltip(typeof(ResRollEstimatedOnTime), "StartingTimeSerigraphyToolTip")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = @"{0:hh\:mm}")]
        public Nullable<System.TimeSpan> StartingTimeSerigraphy { get; set; }

        [DisplayNameLocalized(typeof(ResRollEstimatedOnTime), "CostPerHourRunningSerigraphy")]
        [Tooltip(typeof(ResRollEstimatedOnTime), "CostPerHourRunningSerigraphyToolTip")]
        public string CostPerHourRunningSerigraphy { get; set; }

        [DisplayNameLocalized(typeof(ResRollEstimatedOnTime), "CostPerHourStartingSerigraphy")]
        [Tooltip(typeof(ResRollEstimatedOnTime), "CostPerHourStartingSerigraphyToolTip")]
        public string CostPerHourStartingSerigraphy { get; set; }

        [DisplayNameLocalized(typeof(ResRollEstimatedOnTime), "AvarageRunPerHourSerigraphy")]
        [Tooltip(typeof(ResRollEstimatedOnTime), "AvarageRunPerHourSerigraphyToolTip")]
        public Nullable<long> AvarageRunPerHourSerigraphy { get; set; }

        [DisplayNameLocalized(typeof(ResRollEstimatedOnTime), "StartingTimeFoilStamping")]
        [Tooltip(typeof(ResRollEstimatedOnTime), "StartingTimeFoilStampingToolTip")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = @"{0:hh\:mm}")]
        public Nullable<System.TimeSpan> StartingTimeFoilStamping { get; set; }

        [DisplayNameLocalized(typeof(ResRollEstimatedOnTime), "CostPerHourRunningFoilStamping")]
        [Tooltip(typeof(ResRollEstimatedOnTime), "CostPerHourRunningFoilStampingToolTip")]
        public string CostPerHourRunningFoilStamping { get; set; }

        [DisplayNameLocalized(typeof(ResRollEstimatedOnTime), "CostPerHourStartingFoilStamping")]
        [Tooltip(typeof(ResRollEstimatedOnTime), "CostPerHourStartingFoilStampingToolTip")]
        public string CostPerHourStartingFoilStamping { get; set; }

        [DisplayNameLocalized(typeof(ResRollEstimatedOnTime), "AvarageRunPerHourFoilStamping")]
        [Tooltip(typeof(ResRollEstimatedOnTime), "AvarageRunPerHourFoilStampingToolTip")]
        public Nullable<long> AvarageRunPerHourFoilStamping { get; set; }
    }
}
using PapiroMVC.Models.Resources.TaskExecutor;
using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class DigitalOnTime_MetaData : TaskEstimatedOnTime_MetaData
    {
        [DisplayNameLocalized(typeof(ResTaskEstimatedOnTime), "CostPerHourRunningBW")]
        [Tooltip(typeof(ResTaskEstimatedOnTime), "CostPerHourRunningBWToolTip")]
        [RegularExpressionLocalized(typeof(ResTaskEstimatedOnTime), "CurrencyValidation", "CurrencyValidationError")]
        public Nullable<double> CostPerHourRunningBW { get; set; }
        [DisplayNameLocalized(typeof(ResTaskEstimatedOnTime), "CostPerHourStartingBW")]
        [Tooltip(typeof(ResTaskEstimatedOnTime), "CostPerHourStartingBWToolTip")]
        [RegularExpressionLocalized(typeof(ResTaskEstimatedOnTime), "CurrencyValidation", "CurrencyValidationError")]
        public Nullable<double> CostPerHourStartingBW { get; set; }
    }
}
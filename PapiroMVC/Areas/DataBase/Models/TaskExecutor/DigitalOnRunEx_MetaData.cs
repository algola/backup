using PapiroMVC.Models.Resources.TaskExecutor;
using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class DigitalOnRun_MetaData : TaskEstimatedOnRun_MetaData
    {
        [DisplayNameLocalized(typeof(ResTaskEstimatedOnUnit), "StartingCost1BW")]
        [Tooltip(typeof(ResTaskEstimatedOnUnit), "StartingCost1BWToolTip")]
        [RegularExpressionLocalized(typeof(ResTaskEstimatedOnUnit), "CurrencyValidation", "CurrencyValidationError")]
        public string StartingCost1BW { get; set; }

        [DisplayNameLocalized(typeof(ResTaskEstimatedOnUnit), "StartingCost2BW")]
        [Tooltip(typeof(ResTaskEstimatedOnUnit), "StartingCost2BWToolTip")]
        [RegularExpressionLocalized(typeof(ResTaskEstimatedOnUnit), "CurrencyValidation", "CurrencyValidationError")]
        public string StartingCost2BW { get; set; }

        [DisplayNameLocalized(typeof(ResTaskEstimatedOnUnit), "UseDifferentCostPerUnitBW")]
        [Tooltip(typeof(ResTaskEstimatedOnUnit), "UseDifferentCostPerUnitBWToolTip")]
        public Nullable<bool> UseDifferentCostPerUnitBW { get; set; }

        [DisplayNameLocalized(typeof(ResTaskEstimatedOnUnit), "CostForfaitBW")]
        [Tooltip(typeof(ResTaskEstimatedOnUnit), "CostForfaitBWToolTip")]
        [RegularExpressionLocalized(typeof(ResTaskEstimatedOnUnit), "CurrencyValidation", "CurrencyValidationError")]
        public string CostForfaitBW { get; set; }

        [DisplayNameLocalized(typeof(ResTaskEstimatedOnUnit), "CostForfaitForSheetBW")]
        [Tooltip(typeof(ResTaskEstimatedOnUnit), "CostForfaitForSheetBWToolTip")]
        [RegularExpressionLocalized(typeof(ResTaskEstimatedOnUnit), "CurrencyValidation", "CurrencyValidationError")]
        public string CostForfaitForSheetBW { get; set; }

    }
}
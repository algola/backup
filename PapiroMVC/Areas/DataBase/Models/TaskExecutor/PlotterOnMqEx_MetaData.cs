using PapiroMVC.Models.Resources.TaskExecutor;
using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{

    public partial class PlotterOnMq_MetaData : TaskEstimatedOnMq_MetaData
    {
        [RegularExpressionLocalized(typeof(ResPlotterOnMq), "CurrencyValidation", "CurrencyValidationError")]
        [DisplayNameLocalized(typeof(ResPlotterOnMq), "CostInkJetLow")]
        [Tooltip(typeof(ResPlotterOnMq), "CostInkJetLowToolTip")]
        public string CostInkJetLow { get; set; }

        [RegularExpressionLocalized(typeof(ResPlotterOnMq), "CurrencyValidation", "CurrencyValidationError")]
        [DisplayNameLocalized(typeof(ResPlotterOnMq), "CostInkJetMed")]
        [Tooltip(typeof(ResPlotterOnMq), "CostInkJetMedToolTip")]
        public string CostInkJetMed { get; set; }

        [RegularExpressionLocalized(typeof(ResPlotterOnMq), "CurrencyValidation", "CurrencyValidationError")]
        [DisplayNameLocalized(typeof(ResPlotterOnMq), "CostInkJetHight")]
        [Tooltip(typeof(ResPlotterOnMq), "CostInkJetHightToolTip")]
        public string CostInkJetHight { get; set; }

        [RegularExpressionLocalized(typeof(ResPlotterOnMq), "CurrencyValidation", "CurrencyValidationError")]
        [DisplayNameLocalized(typeof(ResPlotterOnMq), "CostUVLow")]
        [Tooltip(typeof(ResPlotterOnMq), "CostUVLowToolTip")]
        public string CostUVLow { get; set; }

        [RegularExpressionLocalized(typeof(ResPlotterOnMq), "CurrencyValidation", "CurrencyValidationError")]
        [DisplayNameLocalized(typeof(ResPlotterOnMq), "CostUVMed")]
        [Tooltip(typeof(ResPlotterOnMq), "CostUVMedToolTip")]
        public string CostUVMed { get; set; }

        [RegularExpressionLocalized(typeof(ResPlotterOnMq), "CurrencyValidation", "CurrencyValidationError")]
        [DisplayNameLocalized(typeof(ResPlotterOnMq), "CostUVHight")]
        [Tooltip(typeof(ResPlotterOnMq), "CostUVHightToolTip")]
        public string CostUVHight { get; set; }

        [RegularExpressionLocalized(typeof(ResPlotterOnMq), "CurrencyValidation", "CurrencyValidationError")]
        [DisplayNameLocalized(typeof(ResPlotterOnMq), "CostWhite")]
        [Tooltip(typeof(ResPlotterOnMq), "CostWhiteToolTip")]
        public string CostWhite { get; set; }

        [RegularExpressionLocalized(typeof(ResPlotterOnMq), "CurrencyValidation", "CurrencyValidationError")]
        [DisplayNameLocalized(typeof(ResPlotterOnMq), "CostCutting")]
        [Tooltip(typeof(ResPlotterOnMq), "CostCuttingToolTip")]
        public string CostCutting { get; set; }

    }
}

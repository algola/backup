using PapiroMVC.Models.Resources.TaskExecutor;
using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class Flexo_MetaData : Litho_MetaData
    {

        [DisplayNameLocalized(typeof(ResFlexo), "PrintingUnit")]
        [Tooltip(typeof(ResFlexo), "PrintingUnitToolTip")]
        public Nullable<short> PrintingUnit { get; set; }

        [DisplayNameLocalized(typeof(ResFlexo), "PaperFirstStartLenght")]
        [Tooltip(typeof(ResFlexo), "PaperFirstStartLenghtToolTip")]
        public Nullable<long> PaperFirstStartLenght { get; set; }

        [DisplayNameLocalized(typeof(ResFlexo), "PaperSecondStartLenght")]
        [Tooltip(typeof(ResFlexo), "PaperSecondStartLenghtToolTip")]
        public Nullable<long> PaperSecondStartLenght { get; set; }

        [DisplayNameLocalized(typeof(ResFlexo), "Width")]
        [Tooltip(typeof(ResFlexo), "WidthToolTip")]
        public Nullable<double> FlexoWidth { get; set; }


        [RegularExpressionLocalized(typeof(ResFlexo), "CurrencyValidation", "CurrencyValidationError")]
        [DisplayNameLocalized(typeof(ResFlexo), "CostImplant")]
        [Tooltip(typeof(ResFlexo), "CostImplantToolTip")]
        public string CostImplant { get; set; }

        [DisplayNameLocalized(typeof(ResFlexo), "ZMetric")]
        [Tooltip(typeof(ResFlexo), "ZMetricToolTip")]
        public Nullable<bool> ZMetric { get; set; }

    }
}
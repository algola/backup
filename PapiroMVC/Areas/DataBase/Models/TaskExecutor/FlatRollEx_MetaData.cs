using PapiroMVC.Models.Resources.TaskExecutor;
using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class FlatRoll_MetaData : Litho_MetaData
    {

        [DisplayNameLocalized(typeof(ResFlexo), "PaperFirstStartLenght")]
        [Tooltip(typeof(ResFlexo), "PaperFirstStartLenghtToolTip")]
        public Nullable<long> PaperFirstStartLenght { get; set; }

        [DisplayNameLocalized(typeof(ResFlexo), "PaperSecondStartLenght")]
        [Tooltip(typeof(ResFlexo), "PaperSecondStartLenghtToolTip")]
        public Nullable<long> PaperSecondStartLenght { get; set; }

        [RegularExpressionLocalized(typeof(ResFlexo), "CurrencyValidation", "CurrencyValidationError")]
        [DisplayNameLocalized(typeof(ResFlexo), "CostImplantFlat")]
        [Tooltip(typeof(ResFlexo), "CostImplantFlatToolTip")]
        public string CostImplant { get; set; }

        [DisplayNameLocalized(typeof(ResFlexo), "Serigraphy")]
        [Tooltip(typeof(ResFlexo), "SerigraphyToolTip")]
        public Nullable<bool> Serigraphy { get; set; }

        [DisplayNameLocalized(typeof(ResFlexo), "SerigraphyPrintingUnit")]
        [Tooltip(typeof(ResFlexo), "SerigraphyPrintingUnitToolTip")]
        public Nullable<long> SerigraphyPrintingUnit { get; set; }

        [DisplayNameLocalized(typeof(ResFlexo), "FoilStamping")]
        [Tooltip(typeof(ResFlexo), "FoilStampingToolTip")]
        public Nullable<bool> FoilStamping { get; set; }

        [DisplayNameLocalized(typeof(ResFlexo), "FoilStampingPrintingUnit")]
        [Tooltip(typeof(ResFlexo), "FoilStampingPrintingUnitToolTip")]
        public Nullable<long> FoilStampingPrintingUnit { get; set; }

        [DisplayNameLocalized(typeof(ResFlexo), "DieCutting")]
        [Tooltip(typeof(ResFlexo), "DieCuttingToolTip")]
        public Nullable<bool> DieCutting { get; set; }

        [DisplayNameLocalized(typeof(ResFlexo), "ProofSheetFirstStartFlatRoll")]
        [Tooltip(typeof(ResFlexo), "ProofSheetFirstStartFlatRollToolTip")]
        public Nullable<long> ProofSheetFirstStartSerigraphy { get; set; }

        [DisplayNameLocalized(typeof(ResFlexo), "ProofSheetFirstStartFlatRoll")]
        [Tooltip(typeof(ResFlexo), "ProofSheetFirstStartFlatRollToolTip")]
        public Nullable<long> ProofSheetFirstStartFoilStamping { get; set; }

        [DisplayNameLocalized(typeof(ResFlexo), "ProofSheetFirstStartFlatRoll")]
        [Tooltip(typeof(ResFlexo), "ProofSheetFirstStartFlatRollToolTip")]
        public Nullable<long> ProofSheetFirstStartDieCutting { get; set; }

        [RegularExpressionLocalized(typeof(ResFlexo), "CurrencyValidation", "CurrencyValidationError")]
        [DisplayNameLocalized(typeof(ResFlexo), "CostImplant")]
        [Tooltip(typeof(ResFlexo), "CostImplantToolTip")]
        public string CostImplantFoilStamping { get; set; }

        [RegularExpressionLocalized(typeof(ResFlexo), "CurrencyValidation", "CurrencyValidationError")]
        [DisplayNameLocalized(typeof(ResFlexo), "CostImplant")]
        [Tooltip(typeof(ResFlexo), "CostImplantToolTip")]
        public string CostImplantDieCutting { get; set; }
    
    }
}
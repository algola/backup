using PapiroMVC.Models.Resources.Products;
using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public class PrintingHint
    {

        [DisplayNameLocalized(typeof(ResProductPart), "Format")]
        [RegularExpressionLocalizedAttribute(typeof(ResProductPart), "FormatValidation", "FormatValidationError")]
        [Tooltip(typeof(ResProductPart), "FormatToolTip")]
        public string Format { get; set; }

        public string Description { get; set; }

        [DisplayNameLocalized(typeof(ResProductPart), "DCut1")]
        [Tooltip(typeof(ResProductPart), "DCut1ToolTip")]
        public Nullable<double> DCut1 { get; set; }

        [DisplayNameLocalized(typeof(ResProductPart), "DCut2")]
        [Tooltip(typeof(ResProductPart), "DCut2ToolTip")]
        public Nullable<double> DCut2 { get; set; }

        [DisplayNameLocalized(typeof(ResProductPart), "SideOnSide")]
        [Tooltip(typeof(ResProductPart), "SideOnSideToolTip")]
        public Nullable<int> SideOnSide { get; set; }

        [DisplayNameLocalized(typeof(ResProductPart), "BuyingFormat")]
        [Tooltip(typeof(ResProductPart), "BuyingFormatToolTip")]
        public string BuyingFormat { get; set; }

        [DisplayNameLocalized(typeof(ResProductPart), "PrintingFormat")]
        [Tooltip(typeof(ResProductPart), "PrintingFormatToolTip")]
        public string PrintingFormat { get; set; }

    }
}
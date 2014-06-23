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

        public int MaxGain1 { get; set; }
        public int MaxGain2 { get; set; }

        public int MinGain1 { get; set; }
        public int MinGain2 { get; set; }

        public double GainOnSide1 { get; set; }
        public double GainOnSide2 { get; set; }
        public double CalculatedGain { get; set; }

        public double DeltaDCut2 { get; set; }

        public double Z
        {
            get
            {
                double a = PrintingFormat.GetSide2() / 2.54;
                return a*8;
            }
        }


    }

    /// <summary>
    /// use for union
    /// </summary>
    class PrintingHintComparer : IEqualityComparer<PrintingHint>
    {
        public bool Equals(PrintingHint p1, PrintingHint p2)
        {

            var ret = p1.BuyingFormat == p2.BuyingFormat &&
                p1.Format == p2.Format &&
                p1.DCut1 == p2.DCut1 &&
                p1.DCut2 == p2.DCut2;

            return ret;
        }

        public int GetHashCode(PrintingHint p)
        {
            return p.GetHashCode();
        }
    }

}
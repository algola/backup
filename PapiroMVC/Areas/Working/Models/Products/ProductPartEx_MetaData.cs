using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using PapiroMVC.Models.Resources.Products;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    public abstract partial class ProductPart_MetaData
    {
        public System.DateTime TimeStampTable { get; set; }
        public string CodProductPart { get; set; }       
        public string CodProduct { get; set; }

        [DisplayNameLocalized(typeof(ResProductPart), "SubjectNumber")]
        [Tooltip(typeof(ResProductPart), "SubjectNumberToolTip")]
        public Nullable<int> SubjectNumber { get; set; }

        [DisplayNameLocalized(typeof(ResProductPart), "ProductPartName")]
        [Tooltip(typeof(ResProductPart), "ProductPartNameToolTip")]
        public string ProductPartName { get; set; }
        
        [DisplayNameLocalized(typeof(ResProductPart), "PrintingType")]
        [Tooltip(typeof(ResProductPart), "PrintingTypeTooltip")]
        public string PrintingType { get; set; }
        
        [DisplayNameLocalized(typeof(ResProductPart), "Format")]
        [RegularExpressionLocalizedAttribute(typeof(ResProductPart), "FormatValidation", "FormatValidationError")]
        [Tooltip(typeof(ResProductPart), "FormatToolTip")]
        public string Format { get; set; }

        [DisplayNameLocalized(typeof(ResProductPart), "Formatmm")]
        [RegularExpressionLocalizedAttribute(typeof(ResProductPart), "FormatValidation", "FormatValidationError")]
        [Tooltip(typeof(ResProductPart), "FormatmmToolTip")]
        public string Formatmm { get; set; }

        [DisplayNameLocalized(typeof(ResProductPart), "ServicesNumber")]
        [Tooltip(typeof(ResProductPart), "ServicesNumberToolTip")]
        public Nullable<int> ServicesNumber { get; set; }

        [DisplayNameLocalized(typeof(ResProductPart), "FormatPersonalized")]
        [Tooltip(typeof(ResProductPart), "FormatPersonalizedToolTip")]
        public string FormatPersonalized { get; set; }

        [DisplayNameLocalized(typeof(ResProductPart), "DCut")]
        [Tooltip(typeof(ResProductPart), "DCutToolTip")]
        public Nullable<double> DCut { get; set; }

        [DisplayNameLocalized(typeof(ResProductPart), "DCut1")]
        [Tooltip(typeof(ResProductPart), "DCut1ToolTip")]
        public Nullable<double> DCut1 { get; set; }

        [DisplayNameLocalized(typeof(ResProductPart), "DCut2")]
        [Tooltip(typeof(ResProductPart), "DCut2ToolTip")]
        public Nullable<double> DCut2 { get; set; }

        [DisplayNameLocalized(typeof(ResProductPart), "IsDCut")]
        [Tooltip(typeof(ResProductPart), "IsDCutToolTip")]
        public Nullable<bool> IsDCut { get; set; }

        [DisplayNameLocalized(typeof(ResProductPart), "SideOnSide")]
        [Tooltip(typeof(ResProductPart), "SideOnSideToolTip")]
        public Nullable<int> SideOnSide { get; set; }

        [DisplayNameLocalized(typeof(ResProductPart), "HaveDCutLimit")]
        [Tooltip(typeof(ResProductPart), "HaveDCutLimitToolTip")]
        public Nullable<bool> HaveDCutLimit { get; set; }

        [DisplayNameLocalized(typeof(ResProductPart), "MaxDCut")]
        [Tooltip(typeof(ResProductPart), "MaxDCutToolTip")]
        public Nullable<double> MaxDCut { get; set; }

        [DisplayNameLocalized(typeof(ResProductPart), "MinDCut")]
        [Tooltip(typeof(ResProductPart), "MinDCutToolTip")]
        public Nullable<double> MinDCut { get; set; }


    }
}
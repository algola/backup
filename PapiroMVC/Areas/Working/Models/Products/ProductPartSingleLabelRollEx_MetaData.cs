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
    public class ProductPartSingleLabelRoll_MetaData : ProductPart_MetaData
    {

        [DisplayNameLocalized(typeof(ResProductPart), "FormatLabel")]
        [RegularExpressionLocalizedAttribute(typeof(ResProductPart), "FormatValidation", "FormatValidationError")]
        [Tooltip(typeof(ResProductPart), "FormatLabelToolTip")]
        public string Format { get; set; }
        
        [DisplayNameLocalized(typeof(ResProductPartSingleLabelRoll), "DCut1")]
        [Tooltip(typeof(ResProductPartSingleLabelRoll), "DCut1ToolTip")]
        public Nullable<double> DCut1 { get; set; }

        [DisplayNameLocalized(typeof(ResProductPartSingleLabelRoll), "DCut2")]
        [Tooltip(typeof(ResProductPartSingleLabelRoll), "DCut2ToolTip")]
        public Nullable<double> DCut2 { get; set; }
        
    }
}
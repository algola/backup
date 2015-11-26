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
    public class ProductPartDoubleLabelRoll_MetaData : ProductPart_MetaData
    {

        [DisplayNameLocalized(typeof(ResProductPart), "FormatA")]
        [RegularExpressionLocalizedAttribute(typeof(ResProductPart), "FormatValidation", "FormatValidationError")]
        [Tooltip(typeof(ResProductPart), "FormatAToolTip")]
        public string FormatA { get; set; }

        [DisplayNameLocalized(typeof(ResProductPart), "FormatB")]
        [RegularExpressionLocalizedAttribute(typeof(ResProductPart), "FormatValidation", "FormatValidationError")]
        [Tooltip(typeof(ResProductPart), "FormatBToolTip")]
        public string FormatB { get; set; }
    }
}
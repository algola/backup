using PapiroMVC.Models.Resources.Products;
using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class ProductPartHotPrintingOption_Metadata : ProductPartTaskOption_Metadata
    {
        [Required(ErrorMessageResourceType = typeof(ResProductPartTask), ErrorMessageResourceName = "RequiredFieldSimple"), 
        DisplayNameLocalized(typeof(ResProductPartTask), "Foil")]
        [Tooltip(typeof(ResProductPartTask), "FoilToolTip")]
        public string Foil { get; set; }

        //[Required(ErrorMessageResourceType = typeof(ResProductPartTask), ErrorMessageResourceName = "RequiredFieldSimple"), 
        //DisplayNameLocalized(typeof(ResProductPartTask), "Format")]
        //[RegularExpressionLocalizedAttribute(typeof(ResProductPart), "FormatValidation", "FormatValidationError")]
        //[Tooltip(typeof(ResProductPartTask), "FormatToolTip")]
        public string Format { get; set; }
    }
}
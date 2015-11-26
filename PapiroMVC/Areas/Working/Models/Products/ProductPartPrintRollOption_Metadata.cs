using PapiroMVC.Models.Resources.Articles;
using PapiroMVC.Models.Resources.Products;
using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public class ProductPartPrintRollOption_Metadata : ProductPartTaskOption_Metadata
    {
        [Required(ErrorMessageResourceType = typeof(ResProductPartTask), ErrorMessageResourceName = "RequiredFieldSimple"),
        DisplayNameLocalized(typeof(ResProductPartTask), "TypeOfTaskPrint")]
        [Tooltip(typeof(ResProductPartTask), "TypeOfTaskPrintToolTip")]
        public string TypeOfTaskPrint { get; set; }

        [Required(ErrorMessageResourceType = typeof(ResProductPartTask), ErrorMessageResourceName = "RequiredFieldSimple"),
        DisplayNameLocalized(typeof(ResProductPartTask), "Ink")]
        [Tooltip(typeof(ResProductPartTask), "InkToolTip")]
        public string Ink { get; set; }

        [Required(ErrorMessageResourceType = typeof(ResProductPartTask), ErrorMessageResourceName = "RequiredFieldSimple"), 
        DisplayNameLocalized(typeof(ResProductPartTask), "Overlay")]
        [Tooltip(typeof(ResProductPartTask), "OverlayToolTip")]
        public string Overlay { get; set; }

    }
}
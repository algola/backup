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
    public class ProductPartSerigraphyOption_Metadata : ProductPartTaskOption_Metadata
    {
        [Required(ErrorMessageResourceType = typeof(ResProductPartTask), ErrorMessageResourceName = "RequiredFieldSimple"), 
        DisplayNameLocalized(typeof(ResProductPartTask), "TypeOfTaskSerigraphy")]
        [Tooltip(typeof(ResProductPartTask), "TypeOfTaskSerigraphyToolTip")]
        public string TypeOfTaskSerigraphy { get; set; }

        [Required(ErrorMessageResourceType = typeof(ResProductPartTask), ErrorMessageResourceName = "RequiredFieldSimple"), 
        DisplayNameLocalized(typeof(ResProductPartTask), "InkSerigraphy")]
        [Tooltip(typeof(ResProductPartTask), "InkSerigraphyToolTip")]
        public string InkSerigraphy { get; set; }

        [Required(ErrorMessageResourceType = typeof(ResProductPartTask), ErrorMessageResourceName = "RequiredFieldSimple"), 
        DisplayNameLocalized(typeof(ResProductPartTask), "Overlay")]
        [Tooltip(typeof(ResProductPartTask), "OverlayToolTip")]
        public string Overlay { get; set; }
    }
}
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
    public partial class ProductBookSheet_MetaData : Product_MetaData
    {
        [Required(ErrorMessageResourceType = typeof(ResProductBookSheet), ErrorMessageResourceName = "RequiredField")]
        [DisplayNameLocalized(typeof(ResProductBookSheet), "Format")]
        [Tooltip(typeof(ResProductBookSheet), "FormatToolTip")]
        public string Format { get; set; }

        [DisplayNameLocalized(typeof(ResProductBookSheet), "FormatPersonalized")]
        [Tooltip(typeof(ResProductBookSheet), "FormatPersonalizedToolTip")]
        public string FormatPersonalized { get; set; }
    }
}
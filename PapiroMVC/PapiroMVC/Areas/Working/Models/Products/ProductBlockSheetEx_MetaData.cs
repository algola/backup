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
    public abstract partial class ProductBlockSheet_MetaData
    {
        [Required(ErrorMessageResourceType = typeof(ResProductBlockSheet), ErrorMessageResourceName = "RequiredField")]
        [DisplayNameLocalized(typeof(ResProductBlockSheet), "Format")]
        [Tooltip(typeof(ResProductBlockSheet), "FormatToolTip")]
        public string Format { get; set; }
    }
}
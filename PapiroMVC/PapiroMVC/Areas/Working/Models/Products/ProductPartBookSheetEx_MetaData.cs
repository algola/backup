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
    public abstract partial class ProductPartBookSheet_MetaData
    {
        
        [DisplayNameLocalized(typeof(ResProductPartBookSheet), "FormatOpened")]
        [Tooltip(typeof(ResProductPartBookSheet), "FormatOpenedToolTip")]
        public string FormatOpened { get; set; }

        [Required(ErrorMessageResourceType = typeof(ResProductPartBookSheet), ErrorMessageResourceName = "RequiredField")]
        [DisplayNameLocalized(typeof(ResProductPartBookSheet), "Pages")]
        [Tooltip(typeof(ResProductPartBookSheet), "PagesToolTip")]
        public Nullable<int> Pages { get; set; }
    }
}
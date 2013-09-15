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
        
        [DisplayNameLocalized(typeof(ResProductPart), "ProductPartName")]
        [Tooltip(typeof(ResProductPart), "ProductPartNameToolTip")]
        public string ProductPartName { get; set; }
        
        [DisplayNameLocalized(typeof(ResProductPart), "PrintingType")]
        [Tooltip(typeof(ResProductPart), "PrintingTypeTooltip")]
        public string PrintingType { get; set; }
        
//        [Required(ErrorMessageResourceType = typeof(ResProductPart), ErrorMessageResourceName = "RequiredField")]
        [DisplayNameLocalized(typeof(ResProductPart), "Format")]
        [Tooltip(typeof(ResProductPart), "FormatToolTip")]
        public string Format { get; set; }
       
        [DisplayNameLocalized(typeof(ResProductPart), "ServicesNumber")]
        [Tooltip(typeof(ResProductPart), "ServicesNumberToolTip")]
        public Nullable<int> ServicesNumber { get; set; }

        [DisplayNameLocalized(typeof(ResProductPart), "FormatPersonalized")]
        [Tooltip(typeof(ResProductPart), "FormatPersonalizedToolTip")]
        public string FormatPersonalized { get; set; }

    }
}
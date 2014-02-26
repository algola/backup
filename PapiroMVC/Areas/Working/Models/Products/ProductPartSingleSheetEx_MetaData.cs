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
    public abstract partial class ProductPartSingleSheet_MetaData : ProductPart_MetaData
    {
        [DisplayNameLocalized(typeof(ResProductPartSingleSheet), "RawCut")]
        [Tooltip(typeof(ResProductPartSingleSheet), "RawCutToolTip")]
        public Nullable<bool> RawCut { get; set; }
    }
}
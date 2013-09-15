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
        [DisplayNameLocalized(typeof(ResProduct), "SubjectNumber")]
        [Tooltip(typeof(ProductPartSingleSheet), "SubjectNumberToolTip")]
        public Nullable<int> SubjectNumber { get; set; }

        [DisplayNameLocalized(typeof(ResProduct), "RawCut")]
        [Tooltip(typeof(ProductPartSingleSheet), "RawCutToolTip")]
        public Nullable<bool> RawCut { get; set; }
    }
}
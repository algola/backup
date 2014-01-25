using PapiroMVC.Models.Resources.MenuProduct;
using PapiroMVC.Models.Resources.Products;
using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class NewProductCommand_MetaData
    {
        [DisplayNameLocalized(typeof(ResNewMenuProductCommand), "NewProduct")]
        [Tooltip(typeof(ResNewMenuProductCommand), "NewProductToolTip")]
        public string NewProduct { get; set; }
    }


}

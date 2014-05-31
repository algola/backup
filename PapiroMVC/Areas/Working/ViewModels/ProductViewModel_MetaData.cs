using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using PapiroMVC.Models.Resources.Products;
using System.ComponentModel.DataAnnotations;
using PapiroMVC.Models.Resources.ViewModels;

namespace PapiroMVC.Models
{
    public abstract partial class ProductViewModel_MetaData
    {

        [DisplayNameLocalized(typeof(ResProductViewModel), "ProductName")]
        [Tooltip(typeof(ResProductViewModel), "ProductNameToolTip")]
        public String ProductName { get; set; }

        [DisplayNameLocalized(typeof(ResProductViewModel), "Customer")]
        [Tooltip(typeof(ResProductViewModel), "CustomerToolTip")]
        public String Customer { get; set; }

        [DisplayNameLocalized(typeof(ResProductViewModel), "Quantities")]
        [Tooltip(typeof(ResProductViewModel), "Quantities")]
        public List<int> Quantities { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    public class ArticleViewModel_Metadata
    {
        [Required(ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "RequiredField"),
            DisplayNameLocalized(typeof(Strings), "SupplierMaker")]
        public string SupplierMaker { get; set; }

        [Required(ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "RequiredField"),
            DisplayNameLocalized(typeof(Strings), "SupplyerBuy")]
        public string SupplyerBuy { get; set; }
    }
}
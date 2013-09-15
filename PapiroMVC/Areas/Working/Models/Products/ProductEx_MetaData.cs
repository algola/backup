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
    public abstract partial class Product_MetaData
    {
        public System.DateTime TimeStampTable { get; set; }

        [DisplayNameLocalized(typeof(ResProduct), "CodProduct")]
        [Tooltip(typeof(ResProduct), "CodProductToolTip")]
        public string CodProduct { get; set; }

        [DisplayNameLocalized(typeof(ResProduct), "ProductName")]
        [Tooltip(typeof(ResProduct), "ProductNameToolTip")]
        public string ProductName { get; set; }


        public string CodMenuProduct { get; set; }
    }
}
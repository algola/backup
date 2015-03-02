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
    public partial class ProductNameGenerator_MetaData
    {

        public System.DateTime TimeStampTable { get; set; }

        [DisplayNameLocalized(typeof(ResProduct), "CodMenuProduct")]
        [Tooltip(typeof(ResProduct), "CodMenuProductToolTip")]
        public string CodMenuProduct { get; set; }

        [DisplayNameLocalized(typeof(ResProduct), "Generator")]
        [Tooltip(typeof(ResProduct), "GeneratorToolTip")]
        public string Generator { get; set; }
       
    }
}
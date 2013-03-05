using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;

namespace PapiroMVC.Models
{
    public class SheetPrintableArticleCost_MetaData
    {
        [DisplayNameLocalized(typeof(Strings),"CostPerKg")]
        [CurrencyLocalized(typeof(Strings), "CurrencyValidation", "CurrencyValidationError")]
        public string CostPerKg { get; set; }
        [DisplayNameLocalized(typeof(Strings),"CostPerSheet")]
        [CurrencyLocalized(typeof(Strings), "CurrencyValidation", "CurrencyValidationError")]
        public string CostPerSheet { get; set; }
    }
}
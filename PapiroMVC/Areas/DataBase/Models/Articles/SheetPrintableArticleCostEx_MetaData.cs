using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using PapiroMVC.Models.Resources.Articles;

namespace PapiroMVC.Models
{
    public class SheetPrintableArticleCost_MetaData : ArticleCost_MetaData
    {
        [DisplayNameLocalized(typeof(ResSheetPrintableArticleCost), "CostPerKg")]
        [RegularExpressionLocalized(typeof(ResSheetPrintableArticleCost), "CurrencyValidation", "CurrencyValidationError")]
        public string CostPerKg { get; set; }
        [DisplayNameLocalized(typeof(ResSheetPrintableArticleCost), "CostPerSheet")]
        [RegularExpressionLocalized(typeof(ResSheetPrintableArticleCost), "CurrencyValidation", "CurrencyValidationError")]
        public string CostPerSheet { get; set; }
    }
}
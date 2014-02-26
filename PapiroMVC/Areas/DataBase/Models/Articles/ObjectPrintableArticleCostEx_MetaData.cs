using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using PapiroMVC.Models.Resources.Articles;


namespace PapiroMVC.Models
{
    public class ObjectPrintableArticleCost_MetaData : ArticleCost_MetaData
    {
        [DisplayNameLocalized(typeof(ResObjectPrintableArticleCost), " CostPerUnit")]
        [RegularExpressionLocalized(typeof(ResObjectPrintableArticleCost), "CurrencyValidation", "CurrencyValidationError")]
        public string CostPerUnit { get; set; }
    }
}
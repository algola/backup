using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using PapiroMVC.Models.Resources.Articles;

namespace PapiroMVC.Models
{
    public class RollPrintableArticleCost_MetaData : ArticleCost_MetaData
    {
        [DisplayNameLocalized(typeof(ResRollPrintableArticleCost), "CostPerMl")]
        [RegularExpressionLocalized(typeof(ResRollPrintableArticleCost), "CurrencyValidation", "CurrencyValidationError")]
        public Nullable<double> CostPerMl { get; set; }
        [DisplayNameLocalized(typeof(ResRollPrintableArticleCost), "CostPerMq")]
        [RegularExpressionLocalized(typeof(ResRollPrintableArticleCost), "CurrencyValidation", "CurrencyValidationError")]
        public Nullable<double> CostPerMq { get; set; }
    }
}
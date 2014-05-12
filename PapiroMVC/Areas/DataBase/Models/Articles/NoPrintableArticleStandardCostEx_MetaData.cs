using PapiroMVC.Models.Resources.Articles;
using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class NoPrintableArticleCostStandard_MetaData : ArticleCost_MetaData
    {
        [DisplayNameLocalized(typeof(ResNoPrintableArticleCost), " CostPerUnit")]
        [RegularExpressionLocalized(typeof(ResNoPrintableArticleCost), "CurrencyValidation", "CurrencyValidationError")]
        public string CostPerUnit { get; set; }
    }
}
using PapiroMVC.Models.Resources.Articles;
using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class NoPrintableArticleCostMq_MetaData : NoPrintableArticleCostStandard_MetaData
    {
        [DisplayNameLocalized(typeof(ResNoPrintableArticleCost), "CostPerMq")]
        [RegularExpressionLocalized(typeof(ResNoPrintableArticleCost), "CurrencyValidation", "CurrencyValidationError")]
        public string CostPerMq { get; set; }
    }
}
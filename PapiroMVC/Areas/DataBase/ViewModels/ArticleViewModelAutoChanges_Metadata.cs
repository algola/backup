using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public class ArticleAutoChanges_Metadata
    {
        [DisplayNameLocalized(typeof(Strings), "SupplierMaker")]
        public string SupplierMaker { get; set; }
        [DisplayNameLocalized(typeof(Strings), "SupplyerBuy")]
        public string SupplyerBuy { get; set; }        
    }

    public class PrintableAutoChanges_Metadata : ArticleAutoChanges_Metadata
    {
        [DisplayNameLocalized(typeof(Strings), "NoBv")]
        public bool NoBv { get; set; }
        [DisplayNameLocalized(typeof(Strings), "Hand")]
        public double Hand { get; set; }
    }

    //one ore other
    [XorFieldRequired(new string[] { "CostPerMl", "CostPerMq" }, typeof(Strings), "RollPrintableArticheAutoChangesFieldValidationError")]
    public class RollPrintableArticleAutoChanges_Metadata : PrintableAutoChanges_Metadata
    {
        [DisplayNameLocalized(typeof(Strings), "MqForafait")]
        public string MqForafait { get; set; }

        [CurrencyLocalized(typeof(Strings), "AutoChangesValidation", "AutoChangesValidationError")]
        [DisplayNameLocalized(typeof(Strings), "CostPerMl")]
        public string CostPerMl { get; set; }

        [CurrencyLocalized(typeof(Strings), "AutoChangesValidation", "AutoChangesValidationError")]
        [DisplayNameLocalized(typeof(Strings), "CostPerMq")]
        public string CostPerMq { get; set; }
    }

}
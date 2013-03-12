using PapiroMVC.Models.Resources.ViewModels;
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
        [DisplayNameLocalized(typeof(ResArticleViewModelAutoChanges), "SupplierMaker")]
        public string SupplierMaker { get; set; }
        [DisplayNameLocalized(typeof(ResArticleViewModelAutoChanges), "SupplyerBuy")]
        public string SupplyerBuy { get; set; }        
    }

    public class PrintableAutoChanges_Metadata : ArticleAutoChanges_Metadata
    {
        [DisplayNameLocalized(typeof(ResArticleViewModelAutoChanges), "NoBv")]
        public bool NoBv { get; set; }
        [DisplayNameLocalized(typeof(ResArticleViewModelAutoChanges), "Hand")]
        public double Hand { get; set; }
    }

    //one ore other
    [XorFieldRequired(new string[] { "CostPerMl", "CostPerMq" }, typeof(ResArticleViewModelAutoChanges), "RollPrintableXorFieldValidationError")]
    public class RollPrintableArticleAutoChanges_Metadata : PrintableAutoChanges_Metadata
    {
        [DisplayNameLocalized(typeof(ResArticleViewModelAutoChanges), "MqForafait")]
        public string MqForafait { get; set; }

        [RegularExpressionLocalized(typeof(ResArticleViewModelAutoChanges), "AutoChangesValidation", "AutoChangesValidationError")]
        [DisplayNameLocalized(typeof(ResArticleViewModelAutoChanges), "CostPerMl")]
        public string CostPerMl { get; set; }

        [RegularExpressionLocalized(typeof(ResArticleViewModelAutoChanges), "AutoChangesValidation", "AutoChangesValidationError")]
        [DisplayNameLocalized(typeof(ResArticleViewModelAutoChanges), "CostPerMq")]
        public string CostPerMq { get; set; }
    }

    //one ore other
    [XorFieldRequired(new string[] { "CostPerKg", "CostPerMl" }, typeof(ResArticleViewModelAutoChanges), "SheetPrintableArticheAutoChangesFieldValidationError")]
    public class SheetPrintableArticleAutoChanges_Metadata : PrintableAutoChanges_Metadata
    {

        [RegularExpressionLocalized(typeof(ResArticleViewModelAutoChanges), "AutoChangesValidation", "AutoChangesValidationError")]
        [DisplayNameLocalized(typeof(ResArticleViewModelAutoChanges), "CostPerKg")]
        public string CostPerKg { get; set; }

        [RegularExpressionLocalized(typeof(ResArticleViewModelAutoChanges), "AutoChangesValidation", "AutoChangesValidationError")]
        [DisplayNameLocalized(typeof(ResArticleViewModelAutoChanges), "CostPerMq")]
        public string CostPerMq { get; set; }

        //flag to indicate what 
        [DisplayNameLocalized(typeof(ResArticleViewModelAutoChanges), "IsCuttedCost")]
        public bool IsCuttedCost { get; set; }
        [DisplayNameLocalized(typeof(ResArticleViewModelAutoChanges), "IsPakedCost")]
        public bool IsPakedCost { get; set; }
        [DisplayNameLocalized(typeof(ResArticleViewModelAutoChanges), "IsPalletCost")]
        public bool IsPalletCost { get; set; }
        [DisplayNameLocalized(typeof(ResArticleViewModelAutoChanges), "IsAllCost")]
        public bool IsAllCost { get; set; }

    }

}
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

    public class RollPrintableArticleAutoChanges_Metadata : PrintableAutoChanges_Metadata
    {
        [DisplayNameLocalized(typeof(Strings), "MqForafait")]
        public Nullable<double> MqForafait { get; set; }
        [DisplayNameLocalized(typeof(Strings), "CostPerMl")]
        public Nullable<double> CostPerMl { get; set; }        
        [DisplayNameLocalized(typeof(Strings), "CostPerMq")]
        public Nullable<double> CostPerMq { get; set; }
    }

}
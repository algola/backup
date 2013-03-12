using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using PapiroMVC.Models.Resources.Articles;


namespace PapiroMVC.Models
{
    public class RollPrintableArticle_MetaData : Printable_MetaData
    {
        [DisplayNameLocalized(typeof(ResRollPrintableArticle), "Width")]
        public Nullable<bool> Width { get; set; }
        [DisplayNameLocalized(typeof(ResRollPrintableArticle), "MqForafait")]
        public Nullable<double> MqForafait { get; set; }
    }
}
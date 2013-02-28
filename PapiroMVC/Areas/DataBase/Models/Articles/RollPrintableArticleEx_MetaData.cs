using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;

namespace PapiroMVC.Models
{
    public class RollPrintableArticle_MetaData : Printable_MetaData
    {
        [DisplayNameLocalized(typeof(Strings), "Width")]
        public Nullable<bool> Width { get; set; }
        [DisplayNameLocalized(typeof(Strings), "MqForafait")]
        public Nullable<double> MqForafait { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using PapiroMVC.Models.Resources.Articles;


namespace PapiroMVC.Models
{
    public class ObjectPrintableArticle_MetaData : Printable_MetaData
    {
        [DisplayNameLocalized(typeof(ResObjectPrintableArticle), "Size")]
        public string Size { get; set; }
        [DisplayNameLocalized(typeof(ResObjectPrintableArticle), "PrintableFormat")]
        public string PrintableFormat { get; set; }
    }
}
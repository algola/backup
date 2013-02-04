using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;

namespace PapiroMVC.Models
{
    public class ObjectPrintableArticle_MetaData : Printable_MetaData
    {
        [DisplayNameLocalized(typeof(Strings),"Size")]
        public string Size { get; set; }
        [DisplayNameLocalized(typeof(Strings),"PrintableFormat")]
        public string PrintableFormat { get; set; }
    }
}
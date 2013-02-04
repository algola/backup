using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;

namespace PapiroMVC.Models
{
    public partial class ArticleCostEx_MetaData
    {
        public System.DateTime TimeStampTable { get; set; }
        [DisplayNameLocalized(typeof(Strings),"CodArticleCost")]
        public string CodArticleCost { get; set; }
        [DisplayNameLocalized(typeof(Strings),"CodArticle")]
        public string CodArticle { get; set; }
    }
}
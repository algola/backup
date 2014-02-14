using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using PapiroMVC.Models.Resources.Articles;

namespace PapiroMVC.Models
{
    public partial class ArticleCost_MetaData
    {
        public System.DateTime TimeStampTable { get; set; }
        [DisplayNameLocalized(typeof(ResArticleCost), "CodArticleCost")]
        public string CodArticleCost { get; set; }
        [DisplayNameLocalized(typeof(ResArticleCost), "CodArticle")]
        public string CodArticle { get; set; }
    }
}
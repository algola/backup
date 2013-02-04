using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;

namespace PapiroMVC.Models
{    
    public partial class Article_MetaData
    {
        public System.DateTime TimeStampTable { get; set; }
        
//        [Required(ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "RequiredField")]
        [DisplayNameLocalized(typeof(Strings), "CodArticle")]
        [Tooltip(typeof(Strings), "CodArticleToolTip")]

        public string CodArticle { get; set; }

        [DisplayNameLocalized(typeof(Strings), "ArticleName")]
        [Tooltip(typeof(Strings), "ArticleNameToolTip")]
        public string ArticleName { get; set; }

        [DisplayNameLocalized(typeof(Strings),"UnitOfMeasure")]
        public string UnitOfMeasure { get; set; }
        public string CodSupplierBuy { get; set; }
        public string CodSupplierMaker { get; set; }
    }
}
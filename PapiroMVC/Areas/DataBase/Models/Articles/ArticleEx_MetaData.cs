using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using PapiroMVC.Models.Resources.Articles;

namespace PapiroMVC.Models
{    
    public partial class Article_MetaData
    {
        //public System.DateTime TimeStampTable { get; set; }
        
//        [Required(ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "RequiredField")]
        [DisplayNameLocalized(typeof(ResArticle), "CodArticle")]
        [Tooltip(typeof(ResArticle), "CodArticleToolTip")]

        public string CodArticle { get; set; }

        [DisplayNameLocalized(typeof(ResArticle), "ArticleName")]
        [Tooltip(typeof(ResArticle), "ArticleNameToolTip")]
        public string ArticleName { get; set; }

        [DisplayNameLocalized(typeof(ResArticle), "UnitOfMeasure")]
        public string UnitOfMeasure { get; set; }
        public string CodSupplierBuy { get; set; }
        public string CodSupplierMaker { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using System.ComponentModel.DataAnnotations;
using PapiroMVC.Models.Resources.ViewModels;

namespace PapiroMVC.Models
{
    public class ArticleViewModel_Metadata
    {
        [Required(ErrorMessageResourceType = typeof(ResArticleViewModelAutoChanges), ErrorMessageResourceName = "RequiredField"),
            DisplayNameLocalized(typeof(ResArticleViewModelAutoChanges), "SupplierMaker")]
        public string SupplierMaker { get; set; }

        [Required(ErrorMessageResourceType = typeof(ResArticleViewModelAutoChanges), ErrorMessageResourceName = "RequiredField"),
            DisplayNameLocalized(typeof(ResArticleViewModelAutoChanges), "SupplyerBuy")]
        public string SupplyerBuy { get; set; }
    }


    public class SheetPrintableArticleViewModel_Metadata : ArticleViewModel_Metadata
    {
        public SheetPrintableArticle Article
        {
            get;
            set;
        }
    }

}
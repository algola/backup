using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{

    public class ArticleViewModelWizard_Metadata : ArticleViewModel_Metadata
    {
        [DisplayNameLocalized(typeof(Strings), "Weight")]
        public string Weights { get; set; }
    }

    public class SheetPrintableArticleViewModelWizard_Metadata : ArticleViewModelWizard_Metadata
    {
        [DisplayNameLocalized(typeof(Strings), "Format")]
        public string Formats { get; set; }
    }

    public class RollPrintableArticleViewModelWizard_Metadata : ArticleViewModelWizard_Metadata
    {
        [DisplayNameLocalized(typeof(Strings), "Width")]
        public string Widths { get; set; }
    }
}
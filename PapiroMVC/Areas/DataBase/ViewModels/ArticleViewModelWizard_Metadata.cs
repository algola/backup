using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{

    public class ArticleViewModelVizard_Metadata : ArticleViewModel_Metadata
    {
        [DisplayNameLocalized(typeof(Strings), "Weight")]
        public string Weights { get; set; }

        [DisplayNameLocalized(typeof(Strings), "Width")]
        public string Widths { get; set; }
    }
}
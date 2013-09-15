using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using PapiroMVC.Models.Resources.Document;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    public abstract partial class Cost_MetaData
    {
        public System.DateTime TimeStampTable { get; set; }

        [DisplayNameLocalized(typeof(ResCost), "CodCost")]
        [Tooltip(typeof(ResCost), "CodCostToolTip")]
        public string CodCost { get; set; }

        [DisplayNameLocalized(typeof(ResCost), "CodDocumentProduct")]
        [Tooltip(typeof(ResCost), "CodDocumentProductToolTip")]
        public string CodDocumentProduct { get; set; }

        [DisplayNameLocalized(typeof(ResCost), "CodProductTask")]
        [Tooltip(typeof(ResCost), "CodProductTaskToolTip")]
        public string CodProductTask { get; set; }

        [DisplayNameLocalized(typeof(ResCost), "CodProductPartTask")]
        [Tooltip(typeof(ResCost), "CodProductPartTaskToolTip")]
        public string CodProductPartTask { get; set; }

        [DisplayNameLocalized(typeof(ResCost), "CodProductPartPrintableArticle")]
        [Tooltip(typeof(ResCost), "CodProductPartPrintableArticleToolTip")]
        public string CodProductPartPrintableArticle { get; set; }

        [DisplayNameLocalized(typeof(ResCost), "DocumentProduct")]
        [Tooltip(typeof(ResCost), "DocumentProductToolTip")]
        public virtual DocumentProduct DocumentProduct { get; set; }

        [DisplayNameLocalized(typeof(ResCost), "ProductPartsPrintableArticle")]
        [Tooltip(typeof(ResCost), "ProductPartsPrintableArticleToolTip")]
        public virtual ProductPartsPrintableArticle ProductPartsPrintableArticle { get; set; }

        [DisplayNameLocalized(typeof(ResCost), "ProductPartTask")]
        [Tooltip(typeof(ResCost), "ProductPartTaskToolTip")]
        public virtual ProductPartTask ProductPartTask { get; set; }

        [DisplayNameLocalized(typeof(ResCost), "ProductTask")]
        [Tooltip(typeof(ResCost), "ProductTaskToolTip")]
        public virtual ProductTask ProductTask { get; set; }



    }
}
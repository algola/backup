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
    public abstract partial class DocumentProduct_MetaData
    {
        public System.DateTime TimeStampTable { get; set; }

        [DisplayNameLocalized(typeof(ResDocumentProduct), "ProductName")]
        [Tooltip(typeof(ResDocumentProduct), "ProductNameToolTip")]
        public string ProductName { get; set; }

        [DisplayNameLocalized(typeof(ResDocumentProduct), "CodProduct")]
        [Tooltip(typeof(ResDocumentProduct), "CodProductToolTip")]
        public string CodProduct { get; set; }

        [DisplayNameLocalized(typeof(ResDocumentProduct), "Quantity")]
        [Tooltip(typeof(ResDocumentProduct), "QuantityToolTip")]
        public Nullable<int> Quantity { get; set; }

        [DisplayNameLocalized(typeof(ResDocumentProduct), "UnitPrice")]
        [Tooltip(typeof(ResDocumentProduct), "UnitPriceToolTip")]
        public string UnitPrice { get; set; }

        [DisplayNameLocalized(typeof(ResDocumentProduct), "Markup")]
        [Tooltip(typeof(ResDocumentProduct), "MarkupToolTip")]
        public string Markup { get; set; }

        [DisplayNameLocalized(typeof(ResDocumentProduct), "TotalAmount")]
        [Tooltip(typeof(ResDocumentProduct), "TotalAmountToolTip")]
        public string TotalAmount { get; set; }

    }
}
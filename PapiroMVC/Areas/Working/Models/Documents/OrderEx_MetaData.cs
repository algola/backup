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
    public abstract partial class Order_MetaData
    {

        [DisplayNameLocalized(typeof(ResDocuments), "OrderNumber")]
        [Tooltip(typeof(ResDocuments), "OrderNumberToolTip")]
        public string OrderNumber { get; set; }

        [DisplayNameLocalized(typeof(ResDocuments), "OrderNumberSerie")]
        [Tooltip(typeof(ResDocuments), "OrderNumberSerieToolTip")]
        public string OrderNumberSerie { get; set; }



    }
}
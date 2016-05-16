using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using PapiroMVC.Models.Resources.Articles;

namespace PapiroMVC.Models
{
    public partial class WarehouseArticleMov_MetaData
    {

        [DisplayNameLocalized(typeof(ResArticle), "Quantity")]
        [Tooltip(typeof(ResArticle), "QuantityToolTip")]
        public Nullable<double> Quantity { get; set; }

        [DisplayNameLocalized(typeof(ResArticle), "TypeOfMov")]
        public Nullable<int> TypeOfMov { get; set; }

        [DisplayNameLocalized(typeof(ResArticle), "UnitOfMeasureMov")]
        [Tooltip(typeof(ResArticle), "UnitOfMeasureMovToolTip")]
        public string UnitOfMeasureMov { get; set; }

        [DisplayNameLocalized(typeof(ResArticle), "UmConversion")]
        [Tooltip(typeof(ResArticle), "UmConversionToolTip")]
        public Nullable<double> UmConversion { get; set; }

        [DisplayNameLocalized(typeof(ResArticle), "DateMov")]
        [Tooltip(typeof(ResArticle), "DateMovToolTip")]
        public Nullable<DateTime> Date { get; set; }

        [DisplayNameLocalized(typeof(ResArticle), "Note")]
        public string Note { get; set; }

    }
}
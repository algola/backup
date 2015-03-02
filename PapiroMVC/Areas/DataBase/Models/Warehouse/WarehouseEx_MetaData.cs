using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using PapiroMVC.Models.Resources.Articles;

namespace PapiroMVC.Models
{
    public partial class Warehouse_MetaData
    {

        [DisplayNameLocalized(typeof(ResArticle), "UnitOfMeasureStore")]
        [Tooltip(typeof(ResArticle), "UnitOfMeasureStoreToolTip")]
        public string UnitOfMeasureStore { get; set; }

        [DisplayNameLocalized(typeof(ResArticle), "UnitOfMeasureMov")]
        [Tooltip(typeof(ResArticle), "UnitOfMeasureMovToolTip")]
        public string UnitOfMeasureMov { get; set; }

        [DisplayNameLocalized(typeof(ResArticle), "UmConversion")]
        [Tooltip(typeof(ResArticle), "UmConversionToolTip")]
        public Nullable<double> UmConversion { get; set; }

        [DisplayNameLocalized(typeof(ResArticle), "QuantityOnHand")]
        [Tooltip(typeof(ResArticle), "QuantityOnHandToolTip")]
        public Nullable<double> QuantityOnHand { get; set; }

        [DisplayNameLocalized(typeof(ResArticle), "Available")]
        [Tooltip(typeof(ResArticle), "AvailableToolTip")]
        public Nullable<double> Available { get; set; }

        [DisplayNameLocalized(typeof(ResArticle), "PotentialQuantityOnHand")]
        [Tooltip(typeof(ResArticle), "PotentialQuantityOnHandToolTip")]
        public Nullable<double> PotentialQuantityOnHand { get; set; }

        [DisplayNameLocalized(typeof(ResArticle), "PotentialAvailable")]
        [Tooltip(typeof(ResArticle), "PotentialAvailableToolTip")]
        public Nullable<double> PotentialAvailable { get; set; }

        [DisplayNameLocalized(typeof(ResArticle), "MinQuantity")]
        [Tooltip(typeof(ResArticle), "MinQuantityToolTip")]
        public Nullable<double> MinQuantity { get; set; }

        [DisplayNameLocalized(typeof(ResArticle), "CodProduct")]
        [Tooltip(typeof(ResArticle), "CodProductToolTip")]
        public string CodProduct { get; set; }

        [DisplayNameLocalized(typeof(ResArticle), "CodArticle")]
        [Tooltip(typeof(ResArticle), "CodArticleToolTip")]
        public string CodArticle { get; set; }
    

    }
}
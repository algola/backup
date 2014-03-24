using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using PapiroMVC.Models.Resources.Products;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    public abstract partial class ProductPartsPrintableArticle_MetaData
    {
        public System.DateTime TimeStampTable { get; set; }
        //[Required(ErrorMessageResourceType = typeof(ResProductPartsPrintableArticle), ErrorMessageResourceName = "RequiredField")]
        [DisplayNameLocalized(typeof(ResProductPartsPrintableArticle), "CodProductPart")]
        [Tooltip(typeof(ResProductPartsPrintableArticle), "CodProductPartToolTip")]
        public string CodProductPart { get; set; }

        //[Required(ErrorMessageResourceType = typeof(ResProductPartsPrintableArticle), ErrorMessageResourceName = "RequiredField")]
        [DisplayNameLocalized(typeof(ResProductPartsPrintableArticle), "CodProductPartPrintableArticle")]
        [Tooltip(typeof(ResProductPartsPrintableArticle), "CCodProductPartPrintableArticleToolTip")]
        public string CodProductPartPrintableArticle { get; set; }

        //[Required(ErrorMessageResourceType = typeof(ResProductPartsPrintableArticle), ErrorMessageResourceName = "RequiredField")]
        [DisplayNameLocalized(typeof(ResProductPartsPrintableArticle), "ProductPartPrintableArticleName")]
        [Tooltip(typeof(ResProductPartsPrintableArticle), "ProductPartPrintableArticleNametToolTip")]
        public string ProductPartPrintableArticleName { get; set; }

        [Required(ErrorMessageResourceType = typeof(ResProductPartsPrintableArticle), ErrorMessageResourceName = "RequiredField")]
        [DisplayNameLocalized(typeof(ResProductPartsPrintableArticle), "TypeOfMaterial")]
        [Tooltip(typeof(ResProductPartsPrintableArticle), "TypeOfMaterialToolTip")]
        public string TypeOfMaterial { get; set; }

        [Required(ErrorMessageResourceType = typeof(ResProductPartsPrintableArticle), ErrorMessageResourceName = "RequiredField")]
        [DisplayNameLocalized(typeof(ResProductPartsPrintableArticle), "NameOfMaterial")]
        [Tooltip(typeof(ResProductPartsPrintableArticle), "NameOfMaterialToolTip")]
        public string NameOfMaterial { get; set; }

        //[Required(ErrorMessageResourceType = typeof(ResProductPartsPrintableArticle), ErrorMessageResourceName = "RequiredField")]
        [DisplayNameLocalized(typeof(ResProductPartsPrintableArticle), "Color")]
        [Tooltip(typeof(ResProductPartsPrintableArticle), "ColorToolTip")]
        public string Color { get; set; }

        //[Required(ErrorMessageResourceType = typeof(ResProductPartsPrintableArticle), ErrorMessageResourceName = "RequiredField")]
        [DisplayNameLocalized(typeof(ResProductPartsPrintableArticle), "Adhesive")]
        [Tooltip(typeof(ResProductPartsPrintableArticle), "AdhesiveToolTip")]
        public string Adhesive { get; set; }

        [Required(ErrorMessageResourceType = typeof(ResProductPartsPrintableArticle), ErrorMessageResourceName = "RequiredField")]
        [DisplayNameLocalized(typeof(ResProductPartsPrintableArticle), "Weight")]
        [Tooltip(typeof(ResProductPartsPrintableArticle), "WeightToolTip")]
        public Nullable<long> Weight { get; set; }
    }
}
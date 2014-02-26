using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using PapiroMVC.Models.Resources.Articles;


namespace PapiroMVC.Models
{
    public class RigidPrintableArticle_MetaData : Printable_MetaData
    {
        [DisplayNameLocalized(typeof(ResRigidPrintableArticle), "Format")]
        public string Format { get; set; }

        [Tooltip(typeof(ResRigidPrintableArticle), "ToNexMqToolTip")]
        [DisplayNameLocalized(typeof(ResRigidPrintableArticle), "ToNexMq")]        
        public Nullable<bool> ToNexMq { get; set; }
        
        [DisplayNameLocalized(typeof(ResRigidPrintableArticle), "FromMinFormat")]
        [Tooltip(typeof(ResRigidPrintableArticle), "FromMinFormatToolTip")]
        [RegularExpressionLocalizedAttribute(typeof(ResRigidPrintableArticle), "FromMinFormatValidation", "FromMinFormatValidationError")]        
        public string FromMinFormat { get; set; }
    }
}
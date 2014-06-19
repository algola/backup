using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using PapiroMVC.Models.Resources.Articles;

namespace PapiroMVC.Models
{
    public partial class Die_MetaData : NoPrintable_MetaData
    {
        
        [DisplayNameLocalized(typeof(ResArticle), "CodDie")]
        public string CodDie { get; set; }
        [Required(ErrorMessageResourceType = typeof(ResArticle), ErrorMessageResourceName = "RequiredField"),
        DisplayNameLocalized(typeof(ResArticle), "PrintingFormat"),
        RegularExpressionLocalizedAttribute(typeof(ResArticle), "FormatValidation", "FormatValidationError")]
        public string PrintingFormat { get; set; }
        [DisplayNameLocalized(typeof(ResArticle), "Width")]
        public Nullable<double> Width { get; set; }
        [DisplayNameLocalized(typeof(ResArticle), "Z")]
        public Nullable<double> Z { get; set; }
        [Required(ErrorMessageResourceType = typeof(ResArticle), ErrorMessageResourceName = "RequiredField"),
        DisplayNameLocalized(typeof(ResArticle), "Format"),
        RegularExpressionLocalizedAttribute(typeof(ResArticle), "FormatValidation", "FormatValidationError")]
        public string Format { get; set; }
        [DisplayNameLocalized(typeof(ResArticle), "DCut1")]
        public Nullable<double> DCut1 { get; set; }
        [DisplayNameLocalized(typeof(ResArticle), "DCut2")]
        public Nullable<double> DCut2 { get; set; }
        [DisplayNameLocalized(typeof(ResArticle), "MaxGain1")]
        public Nullable<int> MaxGain1 { get; set; }
        [DisplayNameLocalized(typeof(ResArticle), "MaxGain2")]
        public Nullable<int> MaxGain2 { get; set; }
        [DisplayNameLocalized(typeof(ResArticle), "Description")]
        public string Description { get; set; }
    }
}
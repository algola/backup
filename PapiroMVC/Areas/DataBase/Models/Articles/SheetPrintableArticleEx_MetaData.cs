using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using PapiroMVC.Models.Resources.Articles;

namespace PapiroMVC.Models
{
    public class SheetPrintableArticle_MetaData : Printable_MetaData
    {
        [Required(ErrorMessageResourceType = typeof(ResSheetPrintableArticle), ErrorMessageResourceName = "RequiredField"), 
        DisplayNameLocalized(typeof(ResSheetPrintableArticle), "Format"),
        RegularExpressionLocalizedAttribute(typeof(ResSheetPrintableArticle),"FormatValidation","FormatValidationError")]        
        public string Format { get; set; }
        
        [DisplayNameLocalized(typeof(ResSheetPrintableArticle), "NoPinza")]
        public Nullable<bool> NoPinza { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(ResSheetPrintableArticle), ErrorMessageResourceName = "RequiredField"), DisplayNameLocalized(typeof(ResSheetPrintableArticle), "SheetPerPacked")]
        public Nullable<long> SheetPerPacked { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(ResSheetPrintableArticle), ErrorMessageResourceName = "RequiredField"), DisplayNameLocalized(typeof(ResSheetPrintableArticle), "SheetPerPallet")]
        public Nullable<long> SheetPerPallet { get; set; }
    }
}
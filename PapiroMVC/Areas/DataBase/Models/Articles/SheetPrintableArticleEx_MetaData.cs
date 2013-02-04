using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;

namespace PapiroMVC.Models
{
    public class SheetPrintableArticle_MetaData : Printable_MetaData
    {
        [Required(ErrorMessageResourceType=typeof(Strings),ErrorMessageResourceName="RequiredField"), DisplayNameLocalized(typeof(Strings),"Format")]
        public string Format { get; set; }
        [DisplayNameLocalized(typeof(Strings),"NoPinza")]
        public Nullable<bool> NoPinza { get; set; }
        [Required(ErrorMessageResourceType=typeof(Strings),ErrorMessageResourceName="RequiredField"), DisplayNameLocalized(typeof(Strings),"SheetPerPacked")]
        public Nullable<long> SheetPerPacked { get; set; }
        [Required(ErrorMessageResourceType=typeof(Strings),ErrorMessageResourceName="RequiredField"), DisplayNameLocalized(typeof(Strings),"SheetPerPallet")]
        public Nullable<long> SheetPerPallet { get; set; }
    }
}
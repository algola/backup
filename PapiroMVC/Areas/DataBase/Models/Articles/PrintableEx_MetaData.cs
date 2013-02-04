using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;

namespace PapiroMVC.Models
{
    public partial class Printable_MetaData : Article_MetaData
    {
        [Required(ErrorMessageResourceType=typeof(Strings),ErrorMessageResourceName="RequiredField"), DisplayNameLocalized(typeof(Strings),"TypeOfMaterial")]
        public string TypeOfMaterial { get; set; }
        [DisplayNameLocalized(typeof(Strings),"Weight")]
        public Nullable<long> Weight { get; set; }
        [DisplayNameLocalized(typeof(Strings),"Color")]
        public string Color { get; set; }
        [DisplayNameLocalized(typeof(Strings),"Thikness")]
        public Nullable<double> Thikness { get; set; }
        [Required(ErrorMessageResourceType=typeof(Strings),ErrorMessageResourceName="RequiredField"), DisplayNameLocalized(typeof(Strings),"NameOfMaterial")]
        public string NameOfMaterial { get; set; }
        [DisplayNameLocalized(typeof(Strings),"NoBv")]
        public Nullable<bool> NoBv { get; set; }
        [DisplayNameLocalized(typeof(Strings),"Hand")]
        public Nullable<double> Hand { get; set; }

    }
}
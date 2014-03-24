using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using PapiroMVC.Models.Resources.Articles;

namespace PapiroMVC.Models
{
    public partial class Printable_MetaData : Article_MetaData
    {
        [Required(ErrorMessageResourceType = typeof(ResPrintable), ErrorMessageResourceName = "RequiredField"),
        DisplayNameLocalized(typeof(ResPrintable), "TypeOfMaterial"),Tooltip(typeof(ResPrintable), "TypeOfMaterialToolTip")]
        public string TypeOfMaterial { get; set; }

        [DisplayNameLocalized(typeof(ResPrintable), "Weight")]
        public Nullable<long> Weight { get; set; }

        [DisplayNameLocalized(typeof(ResPrintable), "Adhesive")]
        [Tooltip(typeof(ResPrintable), "AdhesiveToolTip")]
        public string Adhesive { get; set; }

        [DisplayNameLocalized(typeof(ResPrintable), "NoUseInEstimateCalculation")]
        [Tooltip(typeof(ResPrintable), "NoUseInEstimateCalculation")]
        public Nullable<bool> NoUseInEstimateCalculation { get; set; }


        [DisplayNameLocalized(typeof(ResPrintable), "Color")]
        public string Color { get; set; }

        [DisplayNameLocalized(typeof(ResPrintable), "Thikness")]
        public Nullable<double> Thikness { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(ResPrintable), ErrorMessageResourceName = "RequiredField"),
        DisplayNameLocalized(typeof(ResPrintable), "NameOfMaterial"), Tooltip(typeof(ResPrintable), "NameOfMaterialToolTip")]
        public string NameOfMaterial { get; set; }
        
        [DisplayNameLocalized(typeof(ResPrintable), "NoBv")]
        public Nullable<bool> NoBv { get; set; }
        
        [DisplayNameLocalized(typeof(ResPrintable), "Hand")]
        public Nullable<double> Hand { get; set; }

    }
}
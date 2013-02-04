using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;

namespace PapiroMVC.Models
{
    public class RigidPrintableArticleCost_MetaData
    {
        [Required(ErrorMessageResourceType=typeof(Strings),ErrorMessageResourceName="RequiredField"), DisplayNameLocalized(typeof(Strings),"CostPerMl")]
        public Nullable<double> CostPerMl { get; set; }
        [Required(ErrorMessageResourceType=typeof(Strings),ErrorMessageResourceName="RequiredField"), DisplayNameLocalized(typeof(Strings),"CostPerMq")]
        public Nullable<double> CostPerMq { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using PapiroMVC.Models.Resources.Articles;


namespace PapiroMVC.Models
{
    public class RigidPrintableArticleCost_MetaData : ArticleCost_MetaData
    {        
        [Required(ErrorMessageResourceType = typeof(ResRigidPrintableArticleCost), ErrorMessageResourceName = "RequiredField"),
        DisplayNameLocalized(typeof(ResRigidPrintableArticleCost), "CostPerMq")]
        public Nullable<double> CostPerMq { get; set; }
    }
}
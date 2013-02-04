using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;

namespace PapiroMVC.Models
{
    public class RigidPrintableArticle_MetaData : Printable_MetaData
    {
        [DisplayNameLocalized(typeof(Strings),"CostPerMq")]
        public Nullable<double> CostPerMq { get; set; }
    }
}
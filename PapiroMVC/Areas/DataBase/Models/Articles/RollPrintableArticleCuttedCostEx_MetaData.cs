using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;

namespace PapiroMVC.Models
{
    public class RollPrintableArticleCuttedCost_MetaData : RollPrintableArticleCost_MetaData
    {
        [DisplayNameLocalized(typeof(Strings),"UseTheSameCostOfStandardWidthAfterKg")]
        public Nullable<bool> UseTheSameCostOfStandardWidthAfterKg { get; set; }
        [DisplayNameLocalized(typeof(Strings),"UseTheSameCostOfStandardWidthAfterMl")]
        public Nullable<bool> UseTheSameCostOfStandardWidthAfterMl { get; set; }
        [DisplayNameLocalized(typeof(Strings),"Kg")]
        public Nullable<double> Kg { get; set; }
        [DisplayNameLocalized(typeof(Strings),"Ml")]
        public Nullable<int> Ml { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using PapiroMVC.Models.Resources.Articles;

namespace PapiroMVC.Models
{
    public class RollPrintableArticleCuttedCost_MetaData : RollPrintableArticleCost_MetaData
    {
        [DisplayNameLocalized(typeof(ResRollPrintableArticleCuttedCost),"UseTheSameCostOfStandardWidthAfterKg")]
        public Nullable<bool> UseTheSameCostOfStandardWidthAfterKg { get; set; }
        [DisplayNameLocalized(typeof(ResRollPrintableArticleCuttedCost), "UseTheSameCostOfStandardWidthAfterMl")]
        public Nullable<bool> UseTheSameCostOfStandardWidthAfterMl { get; set; }
        [DisplayNameLocalized(typeof(ResRollPrintableArticleCuttedCost), "Kg")]
        public Nullable<double> Kg { get; set; }
        [DisplayNameLocalized(typeof(ResRollPrintableArticleCuttedCost), "Ml")]
        public Nullable<int> Ml { get; set; }
    }
}
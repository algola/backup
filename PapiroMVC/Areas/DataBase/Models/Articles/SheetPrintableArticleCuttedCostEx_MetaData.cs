using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;

namespace PapiroMVC.Models
{
    public class SheetPrintableArticleCuttedCost_MetaData
    {
        [DisplayNameLocalized(typeof(Strings),"UseTheSameCostOfPalletAfterKg")]
        public Nullable<bool> UseTheSameCostOfPalletAfterKg { get; set; }
        [DisplayNameLocalized(typeof(Strings),"UseTheSameCostOfSheetAfterSheet")]
        public Nullable<bool> UseTheSameCostOfSheetAfterSheet { get; set; }
        [DisplayNameLocalized(typeof(Strings),"KgPallet")]
        public Nullable<int> KgPallet { get; set; }
        [DisplayNameLocalized(typeof(Strings),"KgSheet")]
        public Nullable<int> KgSheet { get; set; }
    }
}
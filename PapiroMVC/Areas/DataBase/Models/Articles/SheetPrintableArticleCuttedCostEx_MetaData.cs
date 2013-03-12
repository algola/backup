using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using PapiroMVC.Models.Resources.Articles;

namespace PapiroMVC.Models
{
    public class SheetPrintableArticleCuttedCost_MetaData : SheetPrintableArticleCost_MetaData
    {
        [DisplayNameLocalized(typeof(ResSheetPrintableArticleCuttedCost), "UseTheSameCostOfPalletAfterKg")]
        public Nullable<bool> UseTheSameCostOfPalletAfterKg { get; set; }
        [DisplayNameLocalized(typeof(ResSheetPrintableArticleCuttedCost), "UseTheSameCostOfSheetAfterSheet")]
        public Nullable<bool> UseTheSameCostOfSheetAfterSheet { get; set; }
        [DisplayNameLocalized(typeof(ResSheetPrintableArticleCuttedCost), "KgPallet")]
        public Nullable<int> KgPallet { get; set; }
        [DisplayNameLocalized(typeof(ResSheetPrintableArticleCuttedCost), "KgSheet")]
        public Nullable<int> KgSheet { get; set; }
    }
}
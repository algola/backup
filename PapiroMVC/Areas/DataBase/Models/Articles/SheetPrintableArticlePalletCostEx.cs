using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    [MetadataType(typeof(SheetPrintableArticlePalletCost_MetaData))]
    [Serializable]
    public partial class SheetPrintableArticlePalletCost : SheetPrintableArticleCost
    {
        public SheetPrintableArticlePalletCost()
        {
            this.TypeOfArticleCost = ArticleCost.ArticleCostType.SheetPrintableArticlePalletCost;
        }

        #region Added Properties

        #endregion

    }
}

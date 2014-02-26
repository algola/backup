using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    [Serializable]
    [MetadataType(typeof(SheetPrintableArticleCuttedCost_MetaData))]
    public partial class SheetPrintableArticleCuttedCost : SheetPrintableArticleCost
    {
        public SheetPrintableArticleCuttedCost()
        {
            this.TypeOfArticleCost = ArticleCost.ArticleCostType.SheetPrintableArticleCuttedCost;
        }

        #region Added Properties

        #endregion

    }
}

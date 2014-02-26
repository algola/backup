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
    [MetadataType(typeof(SheetPrintableArticlePakedCost_MetaData))]
    public partial class SheetPrintableArticlePakedCost : SheetPrintableArticleCost
    {
        public SheetPrintableArticlePakedCost()
        {
            this.TypeOfArticleCost = ArticleCost.ArticleCostType.SheetPrintableArticlePakedCost;
        }

        #region Added Properties

        #endregion

    }
}

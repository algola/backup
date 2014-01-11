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
    [MetadataType(typeof(RollPrintableArticleCuttedCost_MetaData))]
    public partial class RollPrintableArticleCuttedCost : RollPrintableArticleCost
    {
        public RollPrintableArticleCuttedCost()
        {
            this.TypeOfArticleCost = ArticleCost.ArticleCostType.RollPrintableArticleCuttedCost;
        }

        #region Added Properties

        #endregion

    }
}

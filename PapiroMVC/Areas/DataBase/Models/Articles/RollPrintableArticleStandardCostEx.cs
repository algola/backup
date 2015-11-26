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
    [MetadataType(typeof(RollPrintableArticleStandardCost_MetaData))]
    public partial class RollPrintableArticleStandardCost : RollPrintableArticleCost
    {
        public RollPrintableArticleStandardCost()
        {
            this.TypeOfArticleCost = ArticleCost.ArticleCostType.RollPrintableArticleStandardCost;
        }



        #region Added Properties

        #endregion


    }
}

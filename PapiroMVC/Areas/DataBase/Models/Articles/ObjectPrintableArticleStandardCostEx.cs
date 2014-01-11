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
    [MetadataType(typeof(ObjectPrintableArticleStandardCost_MetaData))]
    public partial class ObjectPrintableArticleStandardCost : ObjectPrintableArticleCost
    {
        public ObjectPrintableArticleStandardCost()
        {
            this.TypeOfArticleCost = ArticleCost.ArticleCostType.ObjectPrintableArticleStandardCost;
        }

        #region Added Properties

        #endregion


    }
}

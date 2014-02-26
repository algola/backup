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
    [MetadataType(typeof(RigidPrintableArticleStandardCost_MetaData))]
    public partial class RigidPrintableArticleStandardCost : RigidPrintableArticleCost 
    {
        public RigidPrintableArticleStandardCost()
        {
            this.TypeOfArticleCost = ArticleCost.ArticleCostType.RigidPrintableArticleStandardCost;
        }

        #region Added Properties

        #endregion



    }
}

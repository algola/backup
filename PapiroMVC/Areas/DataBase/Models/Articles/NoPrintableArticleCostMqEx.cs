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
    [MetadataType(typeof(NoPrintableArticleCostMq_MetaData))]
    public partial class NoPrintableArticleCostMq : NoPrintableArticleCostStandard
    {
        public NoPrintableArticleCostMq()
        {
            this.TypeOfArticleCost = ArticleCost.ArticleCostType.NoPrintableArticleCostMq;
        }

        #region Added Properties

        #endregion


    }
}

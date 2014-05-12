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
    [MetadataType(typeof(NoPrintableArticleCostStandard_MetaData))]
    public partial class NoPrintableArticleCostStandard : ArticleCost
    {
        public NoPrintableArticleCostStandard()
        {
            this.TypeOfArticleCost = ArticleCost.ArticleCostType.NoPrintableArticleCostStandard;
        }

        #region Added Properties

        #endregion


    }
}

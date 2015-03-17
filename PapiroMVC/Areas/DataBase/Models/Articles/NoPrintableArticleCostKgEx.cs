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
    [MetadataType(typeof(NoPrintableArticleCostKg_MetaData))]
    public partial class NoPrintableArticleCostKg : NoPrintableArticleCostStandard
    {
        public NoPrintableArticleCostKg()
        {
            this.TypeOfArticleCost = ArticleCost.ArticleCostType.NoPrintableArticleCostKg;
        }

        #region Added Properties

        #endregion


    }
}

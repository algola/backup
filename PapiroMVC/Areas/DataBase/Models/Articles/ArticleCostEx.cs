using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    [Serializable]
    [MetadataType(typeof(ArticleCost_MetaData))]
    public abstract partial class ArticleCost
    {
        #region Proprietà aggiuntive       
        public enum ArticleCostType : int
        {
            SheetPrintableArticlePakedCost = 0,
            SheetPrintableArticlePalletCost = 1,
            SheetPrintableArticleCuttedCost = 2,
            RollPrintableArticleStandardCost = 3,
            RollPrintableArticleCuttedCost = 4,
            RigidPrintableArticleStandardCost = 5,
            ObjectPrintableArticleStandardCost = 6
        }

        public ArticleCostType TypeOfArticleCost
        {
            get;
            protected set;
        }

        #endregion

        public bool IsSelected
        {
            get;
            set;
        }

    }
}

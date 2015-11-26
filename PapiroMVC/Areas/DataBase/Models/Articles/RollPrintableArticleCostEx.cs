using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using PapiroMVC.Validation;
using PapiroMVC.Models.Resources.Articles;
using System.Threading;
using PapiroMVC.Validation;

namespace PapiroMVC.Models
{
    [Serializable]
    public abstract partial class RollPrintableArticleCost : ArticleCost
    {

        public string GetCostPerMq()
        {
            double costPerMq = Convert.ToDouble(CostPerMq, Thread.CurrentThread.CurrentUICulture);
            double costPerKg = Convert.ToDouble(CostPerKg, Thread.CurrentThread.CurrentUICulture);

            RollPrintableArticle article = (RollPrintableArticle)this.Articles;

            if (costPerMq != 0)
            {
                return CostPerMq;
            }
            else
            {
                if (article.Weight + article.SuppWeight != 0)
                {
                    return (costPerKg * (article.Weight ?? 0) / 1000).ToString("#,0.0000", Thread.CurrentThread.CurrentUICulture);
                }
                else
                {
                    return "0";
                }
            }
        }
        #region Added Properties

        #endregion
    }
}


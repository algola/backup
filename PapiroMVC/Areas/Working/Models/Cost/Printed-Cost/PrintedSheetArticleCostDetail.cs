using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class PrintedRollArticleCostDetail : PrintedArticleCostDetail 
    {

        public override void GetCostFromList(IQueryable<Article> articles)
        {
       //     this.ProductPart.Prin        
        }

        public override void CostDetailCostCodeRigen()
        {
            base.CostDetailCostCodeRigen();
        }

    }
}
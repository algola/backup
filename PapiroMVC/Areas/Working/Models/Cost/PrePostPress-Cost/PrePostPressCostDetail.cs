using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class PrePostPressCostDetail : CostDetail, ICloneable
    {

        public override void Copy(CostDetail to)
        {
            base.Copy(to);
        }

        public PrePostPressCostDetail()
        {
            TypeOfCostDetail = CostDetailType.ControlTableCostDetail;
        }

        protected IQueryable<Article> _articles;


    }

}
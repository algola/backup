using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class PrintedArticleCostDetail : CostDetail
    {

        public virtual void GetCostFromList(IQueryable<Article> articles)
        {
            //throw (new NotImplementedException());
        }

        public override void InitCostDetail(IQueryable<TaskExecutor> tskExec, IQueryable<Article> articles)
        {

        }

        public override void CostDetailCostCodeRigen()
        {
            this.TimeStampTable = DateTime.Now;
        }




    }
}
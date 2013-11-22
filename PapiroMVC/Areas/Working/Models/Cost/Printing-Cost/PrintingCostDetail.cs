using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class PrintingCostDetail : CostDetail
    {
        public override void InitCostDetail(IQueryable<TaskExecutor> tskExec, IQueryable<Article> articles)
        {
            base.InitCostDetail(tskExec, articles);

            String codTypeOfTask = String.Empty;

            Console.WriteLine(ProductPart); //= TaskCost.ProductPartTask.ProductPart;
            codTypeOfTask = TaskCost.ProductPartTask.OptionTypeOfTask.CodTypeOfTask;
            this.TaskExecutors = tskExec.Where(x => x.CodTypeOfTask == codTypeOfTask).ToList();
        }


        public override void CostDetailCostCodeRigen()
        {
            base.CostDetailCostCodeRigen();
        }


        public virtual List<PrintedArticleCostDetail> GetRelatedPrintedCostDetail(IQueryable<Article> articles, IQueryable<Cost> costs)
        {

            return null;
        }


    }
}
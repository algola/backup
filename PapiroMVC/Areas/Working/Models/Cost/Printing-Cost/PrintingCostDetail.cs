using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class PrintingCostDetail : CostDetail
    {
        public override void InitCostDetail(IQueryable<TaskExecutor> tskExec, IQueryable<Article> articles, Cost taskCost)
        {
            base.InitCostDetail(tskExec, articles, taskCost);

            String codTypeOfTask = String.Empty;

            Console.WriteLine(ProductPart); //= TaskCost.ProductPartTask.ProductPart;
            codTypeOfTask = TaskCost.ProductPartTask.OptionTypeOfTask.CodTypeOfTask;
            this.TaskExecutors = tskExec.Where(x => x.CodTypeOfTask == codTypeOfTask).ToList();
        }

        public override void InitCostDetail2(IQueryable<TaskExecutor> tskExec, IQueryable<Article> articles)
        {
            base.InitCostDetail2(tskExec, articles);

            String codTypeOfTask = String.Empty;

            Console.WriteLine(ProductPart); //= TaskCost.ProductPartTask.ProductPart;
            codTypeOfTask = TaskCost.ProductPartTask.OptionTypeOfTask.CodTypeOfTask;
            this.TaskExecutors = tskExec.Where(x => x.CodTypeOfTask == codTypeOfTask).ToList();
        }

        public override void CostDetailCostCodeRigen()
        {
            base.CostDetailCostCodeRigen();
        }


    }
}
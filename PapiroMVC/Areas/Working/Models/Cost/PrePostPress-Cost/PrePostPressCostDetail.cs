using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class PrePostPressCostDetail : CostDetail, ICloneable
    {

        public override void InitCostDetail(IQueryable<TaskExecutor> tskExec, IQueryable<Article> articles)
        {

            String codTypeOfTask = String.Empty;
            //Console.WriteLine(ProductPart); //= TaskCost.ProductPartTask.ProductPart;
            codTypeOfTask = TaskCost.ProductPartTask.OptionTypeOfTask.CodTypeOfTask;
            tskExec = TaskExecutor.FilterByTask(tskExec, codTypeOfTask);
            TaskExecutors = tskExec.ToList();

            if (TaskexEcutorSelected == null && CodTaskExecutorSelected != "")
            {
                TaskexEcutorSelected = TaskExecutors.FirstOrDefault(x => x.CodTaskExecutor == CodTaskExecutorSelected);
            }
        }

        public override double Quantity(double qta)
        {
            double quantita = 0;
            int typeOfQ = 0;

            if (Printeres != null)
            {
                foreach (var item in Printeres)
                {
                    quantita += item.TaskCost.Quantity ?? 0;

                    this.TypeOfQuantity = item.TypeOfQuantity;
                }

            }

            return quantita;
        }

        public override void Copy(CostDetail to)
        {
            base.Copy(to);
        }

        public PrePostPressCostDetail()
        {
            TypeOfCostDetail = CostDetailType.PrePostPressCostDetail;
        }

        protected IQueryable<Article> _articles;


    }

}
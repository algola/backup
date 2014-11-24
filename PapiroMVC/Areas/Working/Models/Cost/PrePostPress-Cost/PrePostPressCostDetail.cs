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



        public override double UnitCost(double qta)
        {
            if (!IsValid)
            {
                return 0;
            }

            //devo usare gli avvimaneti, la tiratura totale e i mq
            //passarli ad un metodo della macchina corrente e mi restituisce il costo totale che dividerò per
            //la quantità!!!!
            double total = 0;
            TimeSpan time = new TimeSpan(0, 0, 0);
            CostAndTime totalCT = new CostAndTime();

            try
            {
                totalCT = TaskexEcutorSelected.SetTaskExecutorEstimatedOn.FirstOrDefault().GetCost(codOptionTypeOfTask: TaskCost.ProductPartTask.CodOptionTypeOfTask, starts: Starts ?? 1, makereadis:0, running: Quantity(qta));
                Error = (Error != null && Error != 0 && Error != 2) ? 0 : Error;

                //calcolo del tempo e del costo
                total = totalCT.Cost;
                CalculatedTime = totalCT.Time;

            }
            catch (NullReferenceException)
            {
                total = 0;
                Error = 2;
            }

            if (TaskCost.Quantity != null)
            {
                return total / Quantity(qta);

            }
            else
            {
                return 0;
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
                    quantita += item.TaskCost.QuantityMaterial ?? 0;

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
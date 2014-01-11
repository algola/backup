using PapiroMVC.Validation;
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
        }

        public override void CostDetailCostCodeRigen()
        {
            base.CostDetailCostCodeRigen();
        }

        public virtual List<PrintedArticleCostDetail> GetRelatedPrintedCostDetail(IQueryable<Article> articles, IQueryable<Cost> costs)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void UpdateCoeff()
        {
            base.UpdateCoeff();

            //l'esecutore ci dirà quale tipo di quantità mostrare 
            TaskexEcutorSelected = TaskExecutors.Where(x => x.CodTaskExecutor == CodTaskExecutorSelected).FirstOrDefault();
            TypeOfQuantity = (int)TaskexEcutorSelected.TypeOfQuantity;

            //avviamenti con associazione macchina -> tipo di lavorazione
            //per stampare 1 solo foglio --> offset con + avviamenti o digitale per il fr
            var startsToPrint = TaskexEcutorSelected.Starts(TaskCost.ProductPartTask.CodOptionTypeOfTask);
            var makereadies = ProductPartPrinting.CalculatedStarts;

            //per stampare 1 stampato---> tot messe in macchina etc.. oppure resa e calcolo resa media
            var gain = ProductPartPrinting.CalculatedGain;

            Starts = (int)Math.Ceiling(startsToPrint * makereadies);
            //questo valore deve essere moltiplicato per la quantità per ottenere la tiratura!!! 
            GainForRun = (startsToPrint * makereadies / gain);
            GainForRunForPrintableArticle = (makereadies / gain);

            GainForMqRun = (startsToPrint * ProductPartPrinting.CalculatedMq);
            GainForMqRunForPrintableArticle = (ProductPartPrinting.CalculatedMq);

        }

        public override double UnitCost(double qta)
        {
            //devo usare gli avvimaneti, la tiratura totale e i mq
            //passarli ad un metodo della macchina corrente e mi restituisce il costo totale che dividerò per
            //la quantità!!!!

            double total = 0;
            try
            {
                try
                {
                    total = TaskexEcutorSelected.SetTaskExecutorEstimatedOn.FirstOrDefault().GetCost(TaskCost.ProductPartTask.CodOptionTypeOfTask, Starts ?? 1, 0, Quantity(qta));
                }
                catch (NotImplementedException)
                {
                    total = TaskexEcutorSelected.SetTaskExecutorEstimatedOn.FirstOrDefault().GetCost(TaskCost.ProductPartTask.CodOptionTypeOfTask, Starts ?? 1, Quantity(qta));
                }
            }
            catch (NullReferenceException)
            {
                total = 0;
                var e = new NoTaskEstimatedOnException();
                e.Data.Add("TaskExecutor", TaskexEcutorSelected);
                throw e;
            }

            return (total) / Quantity(qta);

        }

    }
}
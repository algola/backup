using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class PrintingCostDetail : CostDetail
    {

        //some coeff depend on Quntity... so when quantity changes... we want to recalulate
        double quantity;
        public double QuantityProp
        {
            get
            {
                quantity = this.TaskCost.DocumentProduct.Quantity ?? 0;
                return quantity;
            }
        }


        /// <summary>
        /// get the partialview name
        /// </summary>
        public virtual string PartialViewName
        {
            get
            {
                return "_" + TypeOfCostDetail.ToString();
            }
        }

        public override void InitCostDetail(IQueryable<TaskExecutor> tskExec, IQueryable<Article> articles)
        {
            base.InitCostDetail(tskExec, articles);
        }

        public override void CostDetailCostCodeRigen()
        {
            base.CostDetailCostCodeRigen();
        }

        public virtual List<CostDetail> GetRelatedPrintedCostDetail(IQueryable<Article> articles, IQueryable<Cost> costs)
        {
            return null;
        }

        public virtual List<CostDetail> GetRelatedImplantCostDetail(string codProductPartTask, IQueryable<Cost> costs)
        {
            List<CostDetail> lst = new List<CostDetail>();

            var x = new ImplantCostDetail();

            x.ComputedBy = this;
            x.ProductPart = this.ProductPart;

            //devo pescare il costo e associarlo al dettaglio
            if (x.CodCost == null)
            {
                var xxxx = costs.ToList();

                var cost = costs.Where(pp => pp.CodProductPartImplantTask == codProductPartTask).FirstOrDefault();
                //da non usare MAIIII                    x.TaskCost = cost;
                x.CodCost = cost.CodCost;
                x.CodCostDetail = cost.CodCost;

                x.CostDetailCostCodeRigen();
            }

            //GUID
            x.Guid = this.Guid;
            this.Computes.Add(x);
            lst.Add(x);

            return lst;
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

            if (ProductPartPrinting.CalculatedGain == 0)
            {
                Error = 1;

                Starts = 0;
                //questo valore deve essere moltiplicato per la quantità per ottenere la tiratura!!! 
                GainForRun = 0;
                GainForRunForPrintableArticle = 0;

                GainForMqRun = 0;
                GainForMqRunForPrintableArticle = 0;

                RollChanges = 0;

            }
            else
            {
                Error = (Error != null && Error != 0 && Error != 1) ? 0 : Error;

                //avviamenti con associazione macchina -> tipo di lavorazione
                //per stampare 1 solo foglio --> offset con + avviamenti o digitale per il fr
                var startsToPrint = TaskexEcutorSelected.Starts(TaskCost.ProductPartTask.CodOptionTypeOfTask);
                var makereadies = ProductPartPrinting.CalculatedStarts;

                //per stampare 1 stampato---> tot messe in macchina etc.. oppure resa e calcolo resa media
                var gain = ProductPartPrinting.CalculatedGain;

                Starts = (int)Math.Ceiling(startsToPrint * makereadies);
                //questo valore deve essere moltiplicato per la quantità per ottenere la tiratura!!! 
                GainForRun = (startsToPrint * makereadies / gain);

                //questo valore serve per moltiplicarlo per ottenere le battute del materiale necessario
                GainForRunForPrintableArticle = (makereadies / gain);

                //moltiplicato per ottenere i mq di produzione
                GainForMqRun = (startsToPrint * ProductPartPrinting.CalculatedMq);
                GainForMqRunForPrintableArticle = (ProductPartPrinting.CalculatedMqPrintingFormat / gain);

                RollChanges = 0;
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
                Error = (Error != null && Error != 0 && Error != 2) ? 0 : Error;
            }
            catch (NullReferenceException)
            {
                total = 0;
                Error = 2;
            }

            return (total) / Quantity(qta);

        }

    }
}
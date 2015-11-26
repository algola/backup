using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class PrintingCostDetail : CostDetail, ICloneable
    {


        public override void Copy(CostDetail to)
        {
            base.Copy(to);
            
            PrintingCostDetail to2 = (PrintingCostDetail)to;
            
            to2.PrintingFormat = this.PrintingFormat;
            to2.HideBuyingInView = this.HideBuyingInView;

            to = to2;

        }


     //   public Die Die {get;set;}

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


        public override void InitCostDetail(IQueryable<TaskExecutor> tskExec, IQueryable<Article> articles)
        {
            base.InitCostDetail(tskExec, articles);        
        }

        public override void CostDetailCostCodeRigen()
        {
            base.CostDetailCostCodeRigen();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void UpdateCoeff()
        {
            base.UpdateCoeff();

            //l'esecutore ci dirà quale tipo di quantità mostrare 
            TypeOfQuantity = (int)TaskexEcutorSelected.TypeOfQuantity;

            if (ProductPartPrinting.CalculatedGain == 0)
            {
                Error = 1;

                Starts = 0;
                Washes = 0;
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
                var washes = TaskexEcutorSelected.GetWashes(TaskCost.ProductPartTask.CodOptionTypeOfTask);
                var starts = TaskexEcutorSelected.GetStarts(TaskCost.ProductPartTask.CodOptionTypeOfTask);
                
                var makereadies = ProductPartPrinting.CalculatedStarts;

                //per stampare 1 stampato---> tot messe in macchina etc.. oppure resa e calcolo resa media
                var gain = ProductPartPrinting.CalculatedGain;

                Washes = (int)Math.Ceiling(washes) - 1;
                Starts = (int)Math.Ceiling(starts) * makereadies;
                
                //questo valore deve essere moltiplicato per la quantità per ottenere la tiratura!!! 
                GainForRun = (Starts / gain);

                //questo valore serve per moltiplicarlo per ottenere le battute del materiale necessario
                GainForRunForPrintableArticle = (makereadies / gain);

                //moltiplicato per ottenere i mq di produzione
                GainForMqRun = (Starts * ProductPartPrinting.CalculatedMq);


                if (ProductPartPrinting.Part.ProductPartPrintableArticles.FirstOrDefault().RoundTo ?? true)
                {
                    GainForMqRunForPrintableArticle = (ProductPartPrinting.CalculatedMqPrintingFormat / gain);
                }
                else
                {
                    GainForMqRunForPrintableArticle = (ProductPartPrinting.CalculatedMq / gain);
                }

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
            TimeSpan time = new TimeSpan(0, 0, 0);
            CostAndTime totalCT = new CostAndTime();
            try
            {
                try
                {
                    totalCT = TaskexEcutorSelected.SetTaskExecutorEstimatedOn.FirstOrDefault().GetCost(TaskCost.ProductPartTask.CodOptionTypeOfTask, Starts ?? 1, 0, Quantity(qta));
                }
                catch (NotImplementedException)
                {
                    totalCT = TaskexEcutorSelected.SetTaskExecutorEstimatedOn.FirstOrDefault().GetCost(TaskCost.ProductPartTask.CodOptionTypeOfTask, Starts ?? 1, Quantity(qta));
                }

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

            return (total) / Quantity(qta);

        }

    }
}
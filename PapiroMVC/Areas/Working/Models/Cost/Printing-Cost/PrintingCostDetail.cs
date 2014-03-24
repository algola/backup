﻿using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class PrintingCostDetail : CostDetail
    {

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

            if (ProductPartPrinting.CalculatedGain == 0)
            {
                Error = 1;

                Starts = 0;
                //questo valore deve essere moltiplicato per la quantità per ottenere la tiratura!!! 
                GainForRun = 0;
                GainForRunForPrintableArticle = 0;

                GainForMqRun = 0;
                GainForMqRunForPrintableArticle = 0;

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
                GainForRunForPrintableArticle = (makereadies / gain);

                GainForMqRun = (startsToPrint * ProductPartPrinting.CalculatedMq);
                GainForMqRunForPrintableArticle = (ProductPartPrinting.CalculatedMq);
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
﻿using Novacode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class PrintedArticleCostDetail : CostDetail, IPrintDocX
    {

        public override void Copy(CostDetail to)
        {
            base.Copy(to);

            PrintedArticleCostDetail to2 = (PrintedArticleCostDetail)to;

            to2.CostPerUnit = this.CostPerUnit;
            to2.CostTakenFrom = this.CostTakenFrom;

            to = to2;
        }

        protected IQueryable<Article> _articles;

        public virtual void GetCostFromList(IQueryable<Article> articles)
        {
            throw new NotImplementedException();
        }

        public override void InitCostDetail(IQueryable<TaskExecutor> tskExec, IQueryable<Article> articles)
        {
            base.InitCostDetail(tskExec, articles);
            _articles = articles;
        }

        public override void CostDetailCostCodeRigen()
        {
            base.CostDetailCostCodeRigen();
            this.TimeStampTable = DateTime.Now;
        }

        public override void UpdateCoeff()
        {
            base.UpdateCoeff();
            //sperimentale
            ComputedBy.UpdateCoeff();

            Error = ComputedBy.Error;

            Starts = 1;
            try
            {
                //questo valore deve essere moltiplicato per la quantità per ottenere la tiratura!!!
                GainForRun = (double)(this.ComputedBy.GainForRunForPrintableArticle / (double)this.ComputedBy.GainPrintingOnBuying.Makereadies.Average(x => x.CalculatedGain ?? 1));
                GainForMqRun = (double)(this.ComputedBy.GainForMqRunForPrintableArticle);

            }
            catch (Exception)
            {                
                GainForMqRun =0;
                GainForRun = 0;
            }

            if (_articles == null)
            {
                throw (new NullReferenceException());
            }

            GetCostFromList(_articles);

        }

        public override void MergeField(DocX doc)
        {
            base.MergeField(doc);

            //voglio stampare i dati relativi al materiale di stampa
            //questo dovrebbe far ottenere il costo!!!!!!
            var art = ProductPart.ProductPartPrintableArticles.FirstOrDefault(x => x.CodProductPartPrintableArticle == this.TaskCost.CodProductPartPrintableArticle);
            if (art != null)
            {
                art.MergeField(doc);
            }

        //    ComputedBy.MergeField(doc);

        }



    }
}
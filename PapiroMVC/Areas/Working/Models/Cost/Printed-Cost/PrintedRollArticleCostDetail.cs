﻿using Novacode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class PrintedRollArticleCostDetail : PrintedArticleCostDetail, IPrintDocX
    {

        public override void Copy(CostDetail to)
        {
            base.Copy(to);

            PrintedRollArticleCostDetail to2 = (PrintedRollArticleCostDetail)to;

            to2.CostPerMq = this.CostPerMq;
            to2.CostPerMl = this.CostPerMl;

            to = to2;
        }

        public PrintedRollArticleCostDetail()
        {
            TypeOfCostDetail = CostDetailType.PrintedRollArticleCostDetail;
        }

        public override void GetCostFromList(IQueryable<Article> articles)
        {

            List<Article> extract;

            try
            {
                var p = ProductPart.ProductPartPrintableArticles.FirstOrDefault(x => x.CodProductPartPrintableArticle == this.TaskCost.CodProductPartPrintableArticle);
                //questo dovrebbe far ottenere il costo!!!!!!
                extract = articles.GetArticlesByProductPartPrintableArticle(p).ToList();
                
                if (extract.FirstOrDefault() == null)
                {
                    throw new Exception();
                }

                TypeOfQuantity = (int)extract.FirstOrDefault().TypeOfQuantity;

                var art = extract.OfType<RollPrintableArticle>().FirstOrDefault();


                var aCost = art.ArticleCosts.OfType<RollPrintableArticleStandardCost>().FirstOrDefault();
                CostPerMq = ((RollPrintableArticleCost)aCost).GetCostPerMq();   //.CostPerMq;
                CostPerMl = ((RollPrintableArticleCost)aCost).CostPerMl;

            }
            catch (Exception)
            {
             
                //se non trovo il
                //    throw (new NullReferenceException());
                TypeOfQuantity = 3;

                CostPerMq = "0";
                CostPerMl = "0";
            }

        }
                public override void CostDetailCostCodeRigen()
        {
            base.CostDetailCostCodeRigen();
        }

        public override void UpdateCoeff()
        {
            base.UpdateCoeff();
            //voglio calcolare il prezzo al Ml e il prezzo al Mq!!!!
            GetCostFromList(_articles);
        }

        public override double UnitCost(double qta)
        {
            if (!IsValid)
            {
                return 0;
            }

            return (Convert.ToDouble(CostPerMq));
        }

        public override double Quantity(double qta, CostDetail.QuantityType type = CostDetail.QuantityType.NOTypeOfQuantity)
        {
            double ret;

            var typeOfQuantity = type == CostDetail.QuantityType.NOTypeOfQuantity ? TypeOfQuantity : (Nullable<int>)type;

            try
            {
                var extract = _articles.GetArticlesByProductPartPrintableArticle(ProductPart.ProductPartPrintableArticles.FirstOrDefault(x => x.CodProductPartPrintableArticle == this.TaskCost.CodProductPartPrintableArticle));
                var article = (RollPrintableArticle)extract.FirstOrDefault();

            }
            catch (Exception)
            {
                return 0;
            }
            //var ret = base.Quantity(qta);
            //questo dovrebbe far ottenere il costo!!!!!!

            double mqMat = 0;
            double mlMat = 0;
            double runMat = 0;
            double kgMat = 0;


            var thisArticle= ProductPart.ProductPartPrintableArticles.FirstOrDefault(x => x.CodProductPartPrintableArticle == this.TaskCost.CodProductPartPrintableArticle);


            if ((QuantityType)(ComputedBy.TypeOfQuantity ?? 0) == QuantityType.RunLengthMlTypeOfQuantity)
            {
                //prendo i ml li moltiplico per la banda
                mqMat = ComputedBy.Quantity(qta) * ComputedBy.ProductPartPrinting.PrintingFormat.GetSide1() / 100;
                mlMat = ComputedBy.Quantity(qta);

            }
            else
            {
                //devo ottenere i mq totali di materiale stampato ed uso un trucco... voglio il numero di fogli... lo moltiplico per la resa del materiale e per i mq
                var lastTypeOfQuantity = ComputedBy.TypeOfQuantity;
                ComputedBy.TypeOfQuantity = 0;
                mqMat = ComputedBy.Quantity(qta) * (GainForRunForPrintableArticle ?? 1) * ComputedBy.ProductPartPrinting.PrintingFormat.GetSide1() * ComputedBy.ProductPartPrinting.PrintingFormat.GetSide2() / 10000;
                ComputedBy.TypeOfQuantity = lastTypeOfQuantity;

                mlMat = mqMat / (ComputedBy.ProductPartPrinting.PrintingFormat.GetSide1() / 100);
            }

            kgMat = mqMat * thisArticle.Weight ?? 0;
            kgMat /= 1000;

            this.CalculatedMq = mqMat;
            this.CalculatedMl = mlMat;
            this.CalculatedKg = kgMat;
            this.CalculatedRun = runMat;


            switch ((QuantityType)(TypeOfQuantity ?? 0))
            {
                case QuantityType.MqWorkTypeOfQuantity:
                    ret = Math.Ceiling(mqMat);
                    break;
                default:
                    ret = base.Quantity(qta);
                    break;
            }

            return ret;

        }

        public override void MergeField(DocX doc)
        {
            base.MergeField(doc);
        }

    }
}
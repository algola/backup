using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class ImplantMeshCostDetail : ImplantCostDetail
    {
        public ImplantMeshCostDetail()
        {
            TypeOfCostDetail = CostDetailType.ImplantMeshCostDetail;
        }


        public override void UpdateCoeff()
        {
            base.UpdateCoeff();

            //GainForMqRun is mq in printing format
            GainForMqRun = (double)(this.ComputedBy.TaskexEcutorSelected.FormatMax.GetSide1()
                * this.ComputedBy.TaskexEcutorSelected.FormatMax.GetSide2() / 10000);
        }


        public override double UnitCost(double qta)
        {
            double totCostImplant = 0;
            #region read and calculate Ink --> totCostInk

            List<ProductPartSerigraphyOption> optSeris = new List<ProductPartSerigraphyOption>();

            //serigraphy options where we can find the inks and types
            foreach (var item in ComputedBy.TaskCost.ProductPartTask.ProductPartTaskOptions)
            {
                optSeris.Add((ProductPartSerigraphyOption)item);
            }

            foreach (var item in optSeris)
            {
                var typeSeri = (Mesh)_articles.OfType<Mesh>().FirstOrDefault(x => x.ArticleName == item.TypeOfTaskSerigraphy);

                if (typeSeri != null)
                {
                    totCostImplant += (GainForMqRun ?? 0) * Convert.ToDouble(typeSeri.ArticleCosts.OfType<NoPrintableArticleCostMq>().FirstOrDefault().CostPerMq, Thread.CurrentThread.CurrentUICulture);
                }
            }

            #endregion


            if (TypeOfQuantity == (int)CostDetail.QuantityType.NColorPerMqTypeOfQuantity)
            {
                return totCostImplant;
            }
            else
            {
                return Convert.ToDouble(ComputedBy.TaskexEcutorSelected.CostImplant);
            }
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class ImplantHotPrintingCostDetail : ImplantCostDetail
    {
        public ImplantHotPrintingCostDetail()
        {
            TypeOfCostDetail = CostDetailType.ImplantHotPrintingCostDetail;
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

            var tsk = (FlatRoll)ComputedBy.TaskexEcutorSelected;
            var mq = ((RepassRollCostDetail)ComputedBy).CalculatedMqImplant;

            totCostImplant= Convert.ToDouble(tsk.CostImplantFoilStamping) * mq;

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
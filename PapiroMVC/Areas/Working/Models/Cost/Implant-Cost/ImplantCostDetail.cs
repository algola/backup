using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class ImplantCostDetail : CostDetail
    {
        public override void Copy(CostDetail to)
        {
            base.Copy(to);

            ImplantCostDetail to2 = (ImplantCostDetail)to;

            to = to2;

        }


        public ImplantCostDetail()
        {
            TypeOfCostDetail = CostDetailType.ImplantCostDetail;
        }

        protected IQueryable<Article> _articles;

        public virtual void GetCostFromList()
        {

            //throw new NotImplementedException();
        }

        public override void InitCostDetail(IQueryable<TaskExecutor> tskExec, IQueryable<Article> articles)
        {
            base.InitCostDetail(tskExec, articles);
            //_articles = articles;
        }

        public override void CostDetailCostCodeRigen()
        {
            this.TimeStampTable = DateTime.Now;
        }

        public override void UpdateCoeff()
        {
            base.UpdateCoeff();
            //sperimentale
            ComputedBy.UpdateCoeff();

            Error = ComputedBy.Error;
            Starts = 1;

            //GainForRun is number of Implants (clichè)           
            GainForRun = this.ComputedBy.Implants; //gli avviamenti mi danno il numero di impianti

            //GainForMqRun is mq in printing format
            GainForMqRun = (double)(this.ComputedBy.ProductPartPrinting.PrintingFormat.GetSide1()
                * this.ComputedBy.ProductPartPrinting.PrintingFormat.GetSide2() / 10000);

            TypeOfQuantity = (int)ComputedBy.TaskexEcutorSelected.TypeOfImplantQuantity;
            
        }

        public override double Quantity(double qta)
        {
            if (TypeOfQuantity == (int)CostDetail.QuantityType.NumberTypeOfQuantity)
            {
                return Math.Ceiling((GainForRun ?? 0) * 100) / 100;
            }

            //colori * mq f.to stampa
            if (TypeOfQuantity == (int)CostDetail.QuantityType.NColorPerMqTypeOfQuantity)
            {
                // var x= (GainForRun ?? 0) * (GainForMqRun ?? 0);
                return GainForRun ?? 0; //GainForRum -> numero impianti
            }

            return 0;
        }

        public override double UnitCost(double qta)
        {

            if (TypeOfQuantity == (int)CostDetail.QuantityType.NColorPerMqTypeOfQuantity)
            {
                return Convert.ToDouble(ComputedBy.TaskexEcutorSelected.CostImplant) * GainForMqRun??1;
            }
            else
            {
                return Convert.ToDouble(ComputedBy.TaskexEcutorSelected.CostImplant);
            }
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{


    public partial class ControlTableCostDetail : PrePostPressCostDetail, ICloneable
    {

        public override void Copy(CostDetail to)
        {
            base.Copy(to);
        }

        public ControlTableCostDetail()
        {
            TypeOfCostDetail = CostDetailType.ControlTableCostDetail;

        }



        public virtual void GetCostFromList()
        {
            //throw new NotImplementedException();
        }

        public override void InitCostDetail(IQueryable<TaskExecutor> tskExec, IQueryable<Article> articles)
        {
            Error = 0;
            base.InitCostDetail(tskExec, articles);
        }

        public override void CostDetailCostCodeRigen()
        {
            this.TimeStampTable = DateTime.Now;
        }

        public override void UpdateCoeff()
        {
            base.UpdateCoeff();
            //a questo punto vorrei arrivare ad avere

            //devo capire quale tipo quantità usare e che moltiplicatore usare!!!!
            //lo devo salvare in una proprietà del dettaglio costo

            if (Printeres != null)
            {

                double gainForRun = 1;
                foreach (var fromP in this.Printeres)
                {
                    Starts = fromP.GainOnSide1;

                    gainForRun *= fromP.GainForRun ?? 1;
                }

            }

            TypeOfQuantity = 5;  //TaskexEcutorSelected.TypeOfImplantQuantity;
        }

        public override double Quantity(double qta)
        {
            double quantita = 0;
            int typeOfQ = 0;

            if (Printeres != null)
            {
                foreach (var item in Printeres)
                {
                    quantita += item.TaskCost.Quantity ?? 0;
                    this.TypeOfQuantity = item.TypeOfQuantity;
                }

            }

            return quantita;
        }

        public override double UnitCost(double qta)
        {
            if (!IsValid)
            {
                return 0;
            }

            try
            {
                var labelPerRoll = ((ProductPartSingleLabelRoll)this.Printeres.FirstOrDefault().ProductPart).LabelsPerRoll;
                if (labelPerRoll != null)
                {
                    RollChanges = (qta / labelPerRoll) / this.Printeres.FirstOrDefault().ProductPartPrinting.CalculatedSide1Gain;
                }
            }
            catch (Exception)
            {
                RollChanges = 0;
                //throw;
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
                    totalCT = TaskexEcutorSelected.SetTaskExecutorEstimatedOn.FirstOrDefault().GetCost(TaskCost.ProductPartTask.CodOptionTypeOfTask, 1, RollChanges ?? 0, (int)(Starts ?? 0), Quantity(qta));
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

            if (TaskCost.Quantity != null)
            {
                return total / Quantity(qta);

            }
            else
            {
                return 0;
            }


        }


    }
}
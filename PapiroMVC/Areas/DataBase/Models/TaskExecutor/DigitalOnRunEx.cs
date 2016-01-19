using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace PapiroMVC.Models
{
    [Serializable]
    [MetadataType(typeof(DigitalOnRun_MetaData))]
    public partial class DigitalOnRun : TaskEstimatedOnRun
    {

        public override CostAndTime GetCost(string codOptionTypeOfTask, double starts, PrintingColor colors, int makereadis, double running, double weight)
        {
            //#region cost color
            //if (UseDifferentCostPerUnitBW ?? false)
            //{
            //    try
            //    {
            //        costUnit = steps.OfType<CostPerRunStepBW>().Where(x => x.FromUnit <= running && running <= x.ToUnit).FirstOrDefault().AvarageRunPerHour;

            //    }
            //    catch (NullReferenceException)
            //    {
            //        costUnit = Convert.ToDouble(CostPerUnit ?? "0");
            //    }
            //}
            //else
            //{
            //    costUnit = Convert.ToDouble(CostPerUnit ?? "0");
            //}
            //#endregion

            //if (costUnit == null)
            //{
            //    totalCostR = 0;
            //}
            //else
            //{
            //    costUnit = costUnit - ((costUnit / 100) * (long)(deficitRate ?? 0));
            //}

            //var totalA = (totalCostA);
            //var totalR = (totalCostR * running);

            //CostAndTime ct = new CostAndTime { Cost = totalA + totalR, Time = new TimeSpan(0, 0, 0) };


           return base.GetCost(codOptionTypeOfTask, starts, colors, makereadis, running, weight);        
        }

        public override CostAndTime GetCost(string codOptionTypeOfTask, double starts, int makereadis, double running)
        {

            double total;

            if (!(codOptionTypeOfTask.Contains("BW")))
            {

                total = Convert.ToDouble(StartingCost1, Thread.CurrentThread.CurrentUICulture);
                total += Convert.ToDouble(StartingCost2, Thread.CurrentThread.CurrentUICulture) * --starts;

                if (UseDifferentCostPerUnit ?? false)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    total += Convert.ToDouble(CostPerUnit, Thread.CurrentThread.CurrentUICulture) * running;
                }
            }
            else
            {

                total = Convert.ToDouble(StartingCost1BW, Thread.CurrentThread.CurrentUICulture);
                total += Convert.ToDouble(StartingCost2BW, Thread.CurrentThread.CurrentUICulture) * --starts;

                if (UseDifferentCostPerUnit ?? false)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    total += Convert.ToDouble(CostPerUnitBW, Thread.CurrentThread.CurrentUICulture) * running;
                }

            }

            CostAndTime ct = new CostAndTime { Cost = total, Time = new TimeSpan(0,0,0) };

            return ct;

        }

        public DigitalOnRun()
        {
            this.TypeOfEstimatedOn = TaskEstimatedOn.EstimatedOnType.DigitalOnRun;
        }

        #region Proprietà aggiuntive

        #endregion

    }
}

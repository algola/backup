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
    [MetadataType(typeof(TaskEstimatedOnUnit_MetaData))]
    public abstract partial class TaskEstimatedOnUnit : TaskEstimatedOn
    {


        public override CostAndTime GetCost(string codOptionTypeOfTask, double starts, PrintingColor colors, int makereadies, double running, double weight)
        {
            Double totalCostR = new Double();
            var totalCostA = Convert.ToDouble(this.StartingCost1 ?? "0");

            //avviamenti successivi
            for (int i = 0; i < starts - 1; i++)
            {
                totalCostA += Convert.ToDouble(this.StartingCost2 ?? "0");
            }

            Nullable<double> costUnit = null;
            Nullable<double> costUnitBW = null;
            Nullable<double> deficitRate = null;

            if (UseDifferentDeficitOnCostForWeightStep ?? false)
            {
                try
                {
                    deficitRate = Convert.ToDouble(steps.OfType<DeficitForWeightStep>().Where(x => x.FromUnit <= weight && weight <= x.ToUnit).FirstOrDefault().DeficitRate);
                }
                catch (NullReferenceException)
                {
                    deficitRate = 0;
                }
            }
            else
            {
                deficitRate = 0;
            }

            #region cost color
            if (UseDifferentCostPerUnit ?? false)
            {
                try
                {
                    var c = steps.OfType<CostPerRunStep>().Where(x => x.FromUnit <= running && running <= x.ToUnit).FirstOrDefault().CostPerUnit;
                    costUnit = Convert.ToDouble(c);
                }
                catch (NullReferenceException)
                {
                    costUnit = Convert.ToDouble(CostPerUnit??"0");
                }
            }
            else
            {
                costUnit = Convert.ToDouble(CostPerUnit ?? "0");
            }
            #endregion

            if (costUnit == null)
            {
                totalCostR = 0;
            }
            else
            {
                costUnit = costUnit - ((costUnit / 100) * (long)(deficitRate ?? 0));
            }

            totalCostR = costUnit ?? 0;

            var totalA = (totalCostA);
            var totalR = (totalCostR * running);

            CostAndTime ct = new CostAndTime { Cost = totalA + totalR, Time = new TimeSpan(0,0,0) };

            return ct;
        }

        public override CostAndTime GetCost(string codOptionTypeOfTask, double starts, int makereadies, double running)
        {
            return GetCost(codOptionTypeOfTask, starts, new PrintingColor(), makereadies, running, 0);
        }


        #region Proprietà aggiuntive
        #endregion

    }

}

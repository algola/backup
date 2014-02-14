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

        public override double GetCost(string codOptionTypeOfTask, double starts, int makereadis, double running)
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

            return total;

        }

        public DigitalOnRun()
        {
            this.TypeOfEstimatedOn = TaskEstimatedOn.EstimatedOnType.DigitalOnRun;
        }

        #region Proprietà aggiuntive

        #endregion

    }
}

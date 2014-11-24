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
    public  partial class TaskEstimatedOnRun: TaskEstimatedOnUnit
    {

        public TaskEstimatedOnRun()
        {
            this.TypeOfEstimatedOn = TaskEstimatedOn.EstimatedOnType.OnRun;
        }

        public override CostAndTime GetCost(string codOptionTypeOfTask, double starts, int makereadis, double running)
        {
            double total;
            total = Convert.ToDouble(StartingCost1, Thread.CurrentThread.CurrentUICulture);            
            total += Convert.ToDouble(StartingCost2, Thread.CurrentThread.CurrentUICulture) * --starts;

            if (UseDifferentCostPerUnit??false)
            {
                var step = steps.OfType<CostPerRunStep>().Where(x => x.FromUnit <= running && x.ToUnit >= running).FirstOrDefault();
                if (step != null)
                {
                    total += Convert.ToDouble(step.CostPerUnit, Thread.CurrentThread.CurrentUICulture) * running;
                }
                else
                {
                    total += Convert.ToDouble(CostPerUnit, Thread.CurrentThread.CurrentUICulture) * running;
                }
               // throw new NotImplementedException();
            }
            else
            {
                total += Convert.ToDouble(CostPerUnit, Thread.CurrentThread.CurrentUICulture) * running;
            }

            CostAndTime ct = new CostAndTime { Cost = total, Time = new TimeSpan(0,0,0) };

            return ct;
        }

        #region Proprietà aggiuntive
        #endregion

    }

}

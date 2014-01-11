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

        public override double GetCost(string codOptionTypeOfTask, double starts, int makereadis, double running)
        {
            double total;
            total = Convert.ToDouble(StartingCost1, Thread.CurrentThread.CurrentUICulture);            
            total += Convert.ToDouble(StartingCost2, Thread.CurrentThread.CurrentUICulture) * --starts;

            if (UseDifferentCostPerUnit??false)
            {
                throw new NotImplementedException();
            }
            else
            {
                total += Convert.ToDouble(CostPerUnit, Thread.CurrentThread.CurrentUICulture) * running;
            }

            return total;
        }

        #region Proprietà aggiuntive
        #endregion

    }

}

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
    [MetadataType(typeof(TaskEstimatedOnTime_MetaData))]
    public partial class TaskEstimatedOnTime : TaskEstimatedOn
    {

        public override CostAndTime GetCost(string codOptionTypeOfTask, double starts, int makereadies, double running)
        {
            TimeSpan totalTimeR = new TimeSpan(0,0,0);
            var totalTimeA = this.StartingTime1 ?? TimeSpan.Zero;

            for (int i = 0; i < starts - 1; i++)
            {
                totalTimeA += (StartingTime2 ?? TimeSpan.Zero);
            }

            if (UseDifferentRunPerHour ?? false)
            {
                throw new NotImplementedException();
            }
            else
            {
                if (AvarageRunPerHour == null)
                {
                    totalTimeR = TimeSpan.Zero;
                }
                else
                {
                    //ore
                    var tot = (running / AvarageRunPerHour);
                    var hour = (double)Math.Truncate((decimal)tot);
                    var min = (double)Math.Truncate((decimal)((tot - hour) * 60));
                    totalTimeR += TimeSpan.FromHours(hour) + TimeSpan.FromMinutes(min);
                }
            }


            var costA = CostPerHourStarting;
            var totalA = (totalTimeA.TotalMinutes) / 60 * Convert.ToDouble(costA, Thread.CurrentThread.CurrentUICulture);

            var totalR = (totalTimeR.TotalMinutes) / 60 * Convert.ToDouble(CostPerHourRunning, Thread.CurrentThread.CurrentUICulture);

            CostAndTime ct = new CostAndTime { Cost = totalA + totalR, Time = totalTimeA + totalTimeR };

            return ct;
        }

        public TaskEstimatedOnTime()
        {
            this.TypeOfEstimatedOn = TaskEstimatedOn.EstimatedOnType.OnTime;
        }

        #region Proprietà aggiuntive

        #endregion

    }
}

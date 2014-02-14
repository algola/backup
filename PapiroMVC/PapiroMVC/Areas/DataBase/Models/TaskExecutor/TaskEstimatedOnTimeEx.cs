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

        public override double GetCost(string codOptionTypeOfTask, double starts, int makereadis, double running)
        {
            TimeSpan totalTime;
            totalTime = this.StartingTime1 ?? TimeSpan.Zero;

            for (int i = 0; i < starts - 1; i++)
            {
                totalTime += (StartingTime2 ?? TimeSpan.Zero);
            }

            if (UseDifferentRunPerHour ?? false)
            {
                throw new NotImplementedException();
            }
            else
            {
                if (AvarageRunPerHour == null)
                {
                    totalTime = TimeSpan.Zero;
                }
                else
                {
                    //ore
                    var tot = (running / AvarageRunPerHour);
                    var hour = (double)Math.Truncate((decimal)tot);
                    var min = (double)Math.Truncate((decimal)((tot - hour) * 60));
                    totalTime += TimeSpan.FromHours(hour) + TimeSpan.FromMinutes(min);
                }
            }

            return (totalTime.TotalMinutes) / 60 * Convert.ToDouble(CostPerHourRunning, Thread.CurrentThread.CurrentUICulture);
        }

        public TaskEstimatedOnTime()
        {
            this.TypeOfEstimatedOn = TaskEstimatedOn.EstimatedOnType.OnTime;
        }

        #region Proprietà aggiuntive

        #endregion

    }
}
